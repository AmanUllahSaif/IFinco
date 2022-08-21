using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.BLL.Handler
{
    public class StockHandler : GenericRepository<Stock>
    {
        private iFincoDBEntities _context;
        public StockHandler(iFincoDBEntities context) : base(context)
        {
            _context = context;
        }

        public bool BarCodeExits(string barcode, long companyId, long? branchid = null)
        {
            var stocks = _context.Stocks.Where(x => x.BarCode.ToLower().Equals(barcode.ToLower()) && x.CompanyId == companyId && x.BranchId == branchid && x.IsActive);
            bool isExit = stocks != null && stocks.Count() > 0;
            return isExit;
        }

        public Stock FindByBarCode(string barcode, long companyId, long? branchid = null)
        {
            var stocks = _context.Stocks.Where(x => x.BarCode.ToLower().Equals(barcode.ToLower()) && x.CompanyId == companyId && x.BranchId == x.BranchId && x.IsActive).FirstOrDefault();
            Stock stck = new Stock();
            if (stocks != null && stocks.Id > 0)
            {
                stck.BarCode = stocks.BarCode;
                stck.BarCodeImgUrl = stocks.BarCodeImgUrl;
            }
            return stck;
        }

        public decimal DeductStock(long productId, long companyId, decimal qty, long? pacId, long? branchId = null)
        {
            var stocks = List.Where(x => x.IsActive && x.CompanyId == companyId && x.BranchId == branchId && x.ProductId == productId && x.PACId == pacId).OrderBy(x => x.ExpiryDate).ToList();
            decimal cost = 0;
            if (stocks.Count() > 0)
            {
                var count = 1;
                while (qty > 0)
                {
                    var item = stocks[count - 1];
                    if (count == stocks.Count())
                    {
                        item.Quantity -= qty;
                        cost += qty * item.PurchasingPrice;
                        qty = 0;
                    }
                    else
                    {
                        if (item.Quantity > qty)
                        {
                            cost += qty * item.PurchasingPrice;
                            item.Quantity -= qty;
                            qty = 0;
                        }
                        else
                        {
                            cost += qty * item.PurchasingPrice;
                            qty = qty - item.Quantity;
                            item.Quantity = 0;
                        }
                    }
                    Update(item);
                    count++;
                }

            }
            else
            {
                throw new Exception("Out of stock.");
            }
            return cost;
        }

        public void AddStock(long productId, long companyId, decimal qty, decimal amount, long? pacId, long? branchId = null)
        {
            Stock stock = new Stock();
            stock.Quantity = qty;
            stock.SellingPrice = 0;
            stock.PurchasingPrice = amount;
            stock.CompanyId = companyId;
            stock.BranchId = branchId;
            stock.ProductId = productId;
            stock.CreatedBy = 0;
            stock.CreatedOn = DateTime.UtcNow;
            stock.IsActive = true;
            stock.PACId = pacId;
            Add(stock);
        }

        public List<Stock> GetStock(long companyId, long branchId)
        {
            List<Stock> lst = new List<Stock>();
            var tempStock = List.Where(x => x.CompanyId == companyId && x.BranchId == branchId && x.IsActive && (x.ExpiryDate > DateTime.UtcNow.Date || x.ExpiryDate == null));
            var stckGrp = tempStock.GroupBy(x => new { x.ProductId, x.PACId });
            foreach (var item in stckGrp)
            {
                Stock stck = new Stock();
                stck.Quantity = item.Sum(x => x.Quantity);
                stck.PACId = item.FirstOrDefault().PACId;
                stck.ProductAttributeCombination = item.FirstOrDefault().ProductAttributeCombination;
                stck.Product = item.FirstOrDefault().Product;
                stck.ProductId = item.FirstOrDefault().ProductId;
                lst.Add(stck);
            }
            return lst;
        }
        public IEnumerable<Stock> GetDetail(long productId, long? pacId, long companyId, long branchId)
        {
            List<Stock> lst = new List<Stock>();
            return List.Where(x => x.ProductId == productId && x.PACId == pacId && x.Quantity != 0 && x.CompanyId == companyId && x.BranchId == branchId && x.IsActive).OrderByDescending(x => x.Id).OrderBy(x => x.ExpiryDate);
        }

        public IEnumerable<Stock> GetExpiredStock(long companyId, long branchId)
        {
            return List.Where(x => x.IsActive && x.Quantity > 0 && x.ExpiryDate < DateTime.UtcNow.Date && x.CompanyId == companyId && x.BranchId == branchId);
        }
        public IEnumerable<Stock> GetNearExpireStock(long companyId, long branchId)
        {
            var date = DateTime.UtcNow.AddDays(30).Date;
            return List.Where(x => x.IsActive && x.Quantity > 0 && x.ExpiryDate <= date && x.ExpiryDate >= DateTime.UtcNow.Date && x.CompanyId == companyId && x.BranchId == branchId);
        }
    }
}
