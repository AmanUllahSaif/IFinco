//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iFinco.DAL {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MetaDataResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MetaDataResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("iFinco.DAL.MetaDataResource", typeof(MetaDataResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This barcode already exits.
        /// </summary>
        internal static string BarCodeAlready {
            get {
                return ResourceManager.GetString("BarCodeAlready", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This field must be a number..
        /// </summary>
        internal static string DecimalMessage {
            get {
                return ResourceManager.GetString("DecimalMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This email already exits.
        /// </summary>
        internal static string EmailAlready {
            get {
                return ResourceManager.GetString("EmailAlready", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid date formate date should be dd/mm/yyyy.
        /// </summary>
        internal static string InvalidDate {
            get {
                return ResourceManager.GetString("InvalidDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please Enter Valid Email.
        /// </summary>
        internal static string InvalidEmail {
            get {
                return ResourceManager.GetString("InvalidEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This Field is Required.
        /// </summary>
        internal static string RequiredMessege {
            get {
                return ResourceManager.GetString("RequiredMessege", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This field maximum length should be {1}.
        /// </summary>
        internal static string StringLength {
            get {
                return ResourceManager.GetString("StringLength", resourceCulture);
            }
        }
    }
}
