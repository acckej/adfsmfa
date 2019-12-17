﻿//******************************************************************************************************************************************************************************************//
// Copyright (c) 2019 Neos-Sdi (http://www.neos-sdi.com)                                                                                                                                    //                        
//                                                                                                                                                                                          //
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),                                       //
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,   //
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                                                                                   //
//                                                                                                                                                                                          //
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                                                           //
//                                                                                                                                                                                          //
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,                                      //
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,                            //
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                               //
//                                                                                                                                                                                          //
// https://adfsmfa.codeplex.com                                                                                                                                                             //
// https://github.com/neos-sdi/adfsmfa                                                                                                                                                      //
//                                                                                                                                                                                          //
//******************************************************************************************************************************************************************************************//
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Runtime.Serialization;
using System.Management.Automation.Host;
using System.Xml;

namespace Neos.IdentityServer.MultiFactor.Administration
{  

    [Serializable]
    public enum FlatTemplateMode
    {
        Free = 0,                        // (UserFeaturesOptions.BypassDisabled | UserFeaturesOptions.BypassUnRegistered | UserFeaturesOptions.AllowManageOptions | UserFeaturesOptions.AllowChangePassword | UserFeaturesOptions.AllowEnrollment));
        Open = 1,                        // (UserFeaturesOptions.BypassDisabled | UserFeaturesOptions.AllowUnRegistered | UserFeaturesOptions.AllowManageOptions | UserFeaturesOptions.AllowChangePassword | UserFeaturesOptions.AllowEnrollment));
        Default = 2,                     // (UserFeaturesOptions.AllowDisabled | UserFeaturesOptions.AllowUnRegistered | UserFeaturesOptions.AllowManageOptions | UserFeaturesOptions.AllowChangePassword | UserFeaturesOptions.AllowEnrollment));
        Mixed = 3,                       // (UserFeaturesOptions.AllowManageOptions | UserFeaturesOptions.AllowChangePassword | UserFeaturesOptions.AllowEnrollment));
        Managed = 4,                     // (UserFeaturesOptions.BypassDisabled | UserFeaturesOptions.AllowUnRegistered | UserFeaturesOptions.AllowProvideInformations | UserFeaturesOptions.AllowChangePassword);
        Strict = 5,                      // (UserFeaturesOptions.AllowProvideInformations | UserFeaturesOptions.AdministrativeMode);
        Administrative = 6,              // (UserFeaturesOptions.AdministrativeMode);
        Custom = 7                       // Empty 
    }
  
    [Serializable]
    public class FlatConfig
    {
        public bool UseUIPaginated;
        public bool IsDirty { get; set; }
        public int DeliveryWindow { get; set; }
        public int MaxRetries { get; set; }
        public int PinLength { get; set; }
        public int DefaultPin { get; set; }
        public string Issuer { get; set; }
        public bool UseActiveDirectory { get; set; }
        public bool CustomUpdatePassword { get; set; }
        public bool KeepMySelectedOptionOn { get; set; }
        public bool ChangeNotificationsOn { get; set; }         
        public PreferredMethod DefaultProviderMethod { get; set; }
        public ReplayLevel ReplayLevel { get; set; }
        public bool UseOfUserLanguages { get; set; }
        public string DefaultCountryCode { get; set; }
        public string AdminContact { get; set; }
        public UserFeaturesOptions UserFeatures { get; set; }
        public FlatConfigAdvertising AdvertisingDays { get; set; }
        public ADFSUserInterfaceKind UiKind { get; set; }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            AdminContact = cfg.AdminContact;
            IsDirty = cfg.IsDirty;
            DeliveryWindow = cfg.DeliveryWindow;
            MaxRetries = cfg.MaxRetries;
            DefaultPin = cfg.DefaultPin;
            PinLength = cfg.PinLength;
            Issuer = cfg.Issuer;
            UseActiveDirectory = cfg.UseActiveDirectory;
            CustomUpdatePassword = cfg.CustomUpdatePassword;
            DefaultCountryCode = cfg.DefaultCountryCode;
            KeepMySelectedOptionOn = cfg.KeepMySelectedOptionOn;
            ChangeNotificationsOn = cfg.ChangeNotificationsOn;
            DefaultProviderMethod = cfg.DefaultProviderMethod;
            UseOfUserLanguages = cfg.UseOfUserLanguages;
            ReplayLevel = cfg.ReplayLevel;
            AdminContact = cfg.AdminContact;
            UserFeatures = cfg.UserFeatures;
            AdvertisingDays = (FlatConfigAdvertising)cfg.AdvertisingDays;
            UseUIPaginated = cfg.UseUIPaginated;
            UiKind = cfg.UiKind;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            cfg.AdminContact = AdminContact;
            cfg.IsDirty = IsDirty;
            cfg.DeliveryWindow = DeliveryWindow;
            cfg.MaxRetries = MaxRetries;
            cfg.DefaultPin = DefaultPin;
            cfg.PinLength = PinLength;
            cfg.Issuer = Issuer;
            cfg.UseActiveDirectory = UseActiveDirectory;
            cfg.CustomUpdatePassword = CustomUpdatePassword;
            cfg.KeepMySelectedOptionOn = KeepMySelectedOptionOn;
            cfg.ChangeNotificationsOn = ChangeNotificationsOn;
            cfg.DefaultProviderMethod = DefaultProviderMethod;
            cfg.UseOfUserLanguages = UseOfUserLanguages;
            cfg.ReplayLevel = ReplayLevel;
            cfg.DefaultCountryCode = DefaultCountryCode;
            cfg.AdminContact = AdminContact;
            cfg.UserFeatures = UserFeatures;
            cfg.AdvertisingDays = (ConfigAdvertising)AdvertisingDays;
            cfg.UiKind = UiKind;
            cfg.UseUIPaginated = UseUIPaginated;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }

        /// <summary>
        /// SetPolicyTemplate method implmentation
        /// </summary>
        public void SetPolicyTemplate(PSHost host, FlatTemplateMode mode)
        {
            ManagementService.Initialize(true);
            MFAConfig cfg = ManagementService.Config;
            UserTemplateMode md = (UserTemplateMode)mode;
            cfg.UserFeatures = cfg.UserFeatures.SetPolicyTemplate(md);
            if (md != UserTemplateMode.Administrative)
                cfg.KeepMySelectedOptionOn = true;
            else
                cfg.KeepMySelectedOptionOn = false;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }

        /// <summary>
        /// SetTheme method implementation
        /// </summary>
        internal void SetTheme(PSHost host, int _kind, string _theme, bool _dynparam)
        {
            RegistryVersion reg = new RegistryVersion();
            ManagementService.Initialize(true);
            MFAConfig cfg = ManagementService.Config;
            if (reg.IsWindows2019)
            {
                cfg.UiKind = (ADFSUserInterfaceKind)_kind;
                if ((ADFSUserInterfaceKind)_kind == ADFSUserInterfaceKind.Default)
                    cfg.UseUIPaginated = false;
                else
                    cfg.UseUIPaginated = _dynparam;
                ManagementService.ADFSManager.SetADFSTheme(host, _theme, cfg.UseUIPaginated, true);
                ManagementService.ADFSManager.WriteConfiguration(host);
            }
            else
            {
                cfg.UiKind = ADFSUserInterfaceKind.Default;
                cfg.UseUIPaginated = false;
                ManagementService.ADFSManager.SetADFSTheme(host, _theme, false, false);
                ManagementService.ADFSManager.WriteConfiguration(host);
            }
        }

        /// <summary>
        /// SetLibraryVersion method implementation
        /// </summary>
        internal void SetLibraryVersion(PSHost host, int version)
        {
            ManagementService.Initialize(true);
            MFAConfig cfg = ManagementService.Config;
            cfg.KeysConfig.KeyVersion = (SecretKeyVersion)version;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public class FlatConfigSQL
    {
        public string KeyName { get; set; }
        public int CertificateValidity { get; set; }
        public bool IsDirty { get; set; }
        public string ConnectionString { get; set; }
        public int MaxRows { get; set; }
        public bool IsAlwaysEncrypted { get; set; }
        public string ThumbPrint { get; set; }
        public bool CertReuse { get; set; }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            SQLServerHost sql = cfg.Hosts.SQLServerHost;
            IsDirty = cfg.IsDirty;
            ConnectionString = sql.ConnectionString;
            MaxRows = sql.MaxRows;
            IsAlwaysEncrypted = sql.IsAlwaysEncrypted;
            ThumbPrint = sql.ThumbPrint;
            KeyName = sql.KeyName;
            CertReuse = sql.CertReuse;
            CertificateValidity = sql.CertificateValidity;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            SQLServerHost sql = cfg.Hosts.SQLServerHost;
            cfg.IsDirty = IsDirty;
            if (!ManagementService.CheckSQLConnection(ConnectionString))
                throw new ArgumentException(string.Format("Invalid ConnectionString {0} !", ConnectionString));
            sql.ConnectionString = ConnectionString;
            sql.MaxRows = MaxRows;
            sql.IsAlwaysEncrypted = IsAlwaysEncrypted;
            sql.ThumbPrint = ThumbPrint;
            sql.KeyName = KeyName;
            sql.CertReuse = CertReuse;
            sql.CertificateValidity = CertificateValidity;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public class FlatConfigADDS
    {
        public bool IsDirty { get; set; }
        public string DomainAddress { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string KeyAttribute { get; set; }
        public string MailAttribute { get; set; }
        public string PhoneAttribute { get; set; }
        public string MethodAttribute { get; set; }
        public string OverrideMethodAttribute { get; set; }
        public string PinAttribute { get; set; }
        public string EnabledAttribute { get; set; }
        public string PublicKeyCredentialAttribute { get; set; }
        public int MaxRows { get; set; }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            ADDSHost adds = cfg.Hosts.ActiveDirectoryHost;
            IsDirty = cfg.IsDirty;
            Account = adds.Account;
            Password = adds.Password;
            DomainAddress = adds.DomainAddress;
            KeyAttribute = adds.KeyAttribute;
            MailAttribute = adds.MailAttribute;
            PhoneAttribute = adds.PhoneAttribute;
            MethodAttribute = adds.MethodAttribute;
            OverrideMethodAttribute = adds.OverrideMethodAttribute;
            PinAttribute = adds.PinAttribute;
            EnabledAttribute = adds.TotpEnabledAttribute;
            PublicKeyCredentialAttribute = adds.PublicKeyCredentialAttribute;
            MaxRows = adds.MaxRows;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            ADDSHost adds = cfg.Hosts.ActiveDirectoryHost;
            cfg.IsDirty = IsDirty;
            if (!ManagementService.CheckADDSAttribute(DomainAddress, Account, Password, KeyAttribute, 1))
                throw new ArgumentException(string.Format("Attribute {0} not found in forest schema !", KeyAttribute));
            if (!ManagementService.CheckADDSAttribute(DomainAddress, Account, Password, MailAttribute, 1))
                throw new ArgumentException(string.Format("Attribute {0} not found in forest schema !", MailAttribute));
            if (!ManagementService.CheckADDSAttribute(DomainAddress, Account, Password, PhoneAttribute, 1))
                throw new ArgumentException(string.Format("Attribute {0} not found in forest schema !", PhoneAttribute));
            if (!ManagementService.CheckADDSAttribute(DomainAddress, Account, Password, MethodAttribute, 0))
                throw new ArgumentException(string.Format("Attribute {0} not found in forest schema !", MethodAttribute));
            if (!ManagementService.CheckADDSAttribute(DomainAddress, Account, Password, OverrideMethodAttribute, 0))
                throw new ArgumentException(string.Format("Attribute {0} not found in forest schema !", OverrideMethodAttribute));
            if (!ManagementService.CheckADDSAttribute(DomainAddress, Account, Password, PinAttribute, 0))
                throw new ArgumentException(string.Format("Attribute {0} not found in forest schema !", PinAttribute));
            if (!ManagementService.CheckADDSAttribute(DomainAddress, Account, Password, EnabledAttribute, 0))
                throw new ArgumentException(string.Format("Attribute {0} not found in forest schema !", EnabledAttribute));
            if (!ManagementService.CheckADDSAttribute(DomainAddress, Account, Password, PublicKeyCredentialAttribute, 2))
                throw new ArgumentException(string.Format("Attribute {0} not found in forest schema !", PublicKeyCredentialAttribute));

            adds.Account = Account;
            adds.Password = Password;
            adds.DomainAddress = DomainAddress;
            adds.KeyAttribute = KeyAttribute;
            adds.MailAttribute = MailAttribute;
            adds.PhoneAttribute = PhoneAttribute;
            adds.MethodAttribute = MethodAttribute;
            adds.OverrideMethodAttribute = OverrideMethodAttribute;
            adds.PinAttribute = PinAttribute;
            adds.TotpEnabledAttribute = EnabledAttribute;
            adds.PublicKeyCredentialAttribute = PublicKeyCredentialAttribute;
            adds.MaxRows = MaxRows;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public class FlatKeysConfig
    {
        public bool IsDirty { get; set; }
        public KeyGeneratorMode KeyGenerator  { get; set; }
        public KeySizeMode KeySize { get; set; }
        public SecretKeyFormat KeysFormat  { get; set; }
        public SecretKeyVersion LibVersion { get; set; }
        public string CertificateThumbprint  { get; set; }
        public int CertificateValidity  { get; set; }
        public FlatExternalKeyManager ExternalKeyManager  { get; set; }
        public string XORSecret { get; set; }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            KeysManagerConfig keys = cfg.KeysConfig;
            IsDirty = cfg.IsDirty;
            this.CertificateThumbprint = keys.CertificateThumbprint;
            this.CertificateValidity = keys.CertificateValidity;
            this.KeysFormat = keys.KeyFormat;
            this.KeyGenerator = keys.KeyGenerator;
            this.LibVersion = keys.KeyVersion;
            this.KeySize = keys.KeySize;
            this.XORSecret = keys.XORSecret;
            this.ExternalKeyManager = (FlatExternalKeyManager)keys.ExternalKeyManager;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            KeysManagerConfig keys = cfg.KeysConfig;
            cfg.IsDirty = true;
            keys.CertificateThumbprint = this.CertificateThumbprint;
            keys.CertificateValidity = this.CertificateValidity;
            keys.KeyFormat = this.KeysFormat;
            keys.KeyGenerator = this.KeyGenerator;
            keys.KeyVersion = this.LibVersion;
            keys.KeySize = this.KeySize;
            keys.XORSecret = this.XORSecret;
            keys.ExternalKeyManager = (ExternalKeyManagerConfig)this.ExternalKeyManager;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public class FlatExternalKeyManager
    {
        public string FullQualifiedImplementation { get; set; }
        public string ConnectionString { get; set; }
        public bool IsAlwaysEncrypted { get; set; }
        public string KeyName { get; set; }
        public string ThumbPrint { get; set; }
        public int CertificateValidity { get; set; }
        public bool CertReuse { get; set; }
        public string Parameters { get; set; }

        public static explicit operator ExternalKeyManagerConfig(FlatExternalKeyManager mgr)
        {
            if (mgr == null)
                return null;
            else
            {
                ExternalKeyManagerConfig ret = new ExternalKeyManagerConfig
                {
                    FullQualifiedImplementation = mgr.FullQualifiedImplementation,
                    ConnectionString = mgr.ConnectionString,
                    IsAlwaysEncrypted = mgr.IsAlwaysEncrypted,
                    ThumbPrint = mgr.ThumbPrint,
                    KeyName = mgr.KeyName
                };
                ret.Parameters.Data = mgr.Parameters;
                ret.CertificateValidity = mgr.CertificateValidity;
                ret.CertReuse = mgr.CertReuse;
                return ret;
            }
        }

        public static explicit operator FlatExternalKeyManager(ExternalKeyManagerConfig mgr)
        {
            if (mgr == null)
                return null;
            else
            {
                FlatExternalKeyManager ret = new FlatExternalKeyManager
                {
                    FullQualifiedImplementation = mgr.FullQualifiedImplementation,
                    ConnectionString = mgr.ConnectionString,
                    IsAlwaysEncrypted = mgr.IsAlwaysEncrypted,
                    ThumbPrint = mgr.ThumbPrint,
                    KeyName = mgr.KeyName,
                    Parameters = mgr.Parameters.Data,
                    CertificateValidity = mgr.CertificateValidity,
                    CertReuse = mgr.CertReuse
                };
                return ret;
            }
        }

        public void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            KeysManagerConfig otp = cfg.KeysConfig;
            this.FullQualifiedImplementation = otp.ExternalKeyManager.FullQualifiedImplementation;
            this.ThumbPrint = otp.ExternalKeyManager.ThumbPrint;
            this.KeyName = otp.ExternalKeyManager.KeyName;
            this.CertificateValidity = otp.ExternalKeyManager.CertificateValidity;
            this.CertReuse = otp.ExternalKeyManager.CertReuse;
            this.ConnectionString = otp.ExternalKeyManager.ConnectionString;
            this.IsAlwaysEncrypted = otp.ExternalKeyManager.IsAlwaysEncrypted;
            this.Parameters = otp.ExternalKeyManager.Parameters.Data;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            KeysManagerConfig otp = cfg.KeysConfig;
            otp.ExternalKeyManager.FullQualifiedImplementation = this.FullQualifiedImplementation;
            otp.ExternalKeyManager.ThumbPrint = this.ThumbPrint;
            otp.ExternalKeyManager.KeyName = this.KeyName;
            otp.CertificateValidity = this.CertificateValidity;
            otp.ExternalKeyManager.CertReuse = this.CertReuse;
            otp.ExternalKeyManager.ConnectionString = this.ConnectionString;
            otp.ExternalKeyManager.IsAlwaysEncrypted = this.IsAlwaysEncrypted;
            otp.ExternalKeyManager.Parameters.Data = this.Parameters;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public abstract class FlatBaseProvider
    {
        private string _cdata;
        public bool IsDirty { get; set; }
        public bool Enabled { get; set; }
        public bool IsRequired { get; set; }
        public bool EnrollWizard { get; set; }
        public bool PinRequired { get; set; }
        public ForceWizardMode ForceWizard { get; set; }
        public string FullQualifiedImplementation { get; set; }

        /// <summary>
        /// Parameters property
        /// </summary>
        public string Parameters
        {
            get
            {
                return _cdata;
            }
            set
            {
                _cdata = value;
            }
        }

        public abstract PreferredMethod Kind { get; }
        public abstract void Load(PSHost host);
        public abstract void Update(PSHost host);

        /// <summary>
        /// CkeckUpdate method implementation
        /// </summary>
        public virtual void CheckUpdates(PSHost host)
        {
            IExternalProvider prov = RuntimeAuthProvider.GetProviderInstance(Kind);
            if (prov.Name.Equals("Neos.Provider.Plug.External"))
                return;
            if (prov != null)
            {
                if ((!prov.AllowDisable) && (!this.Enabled))
                    throw new Exception("This Provider cannot be Disabled !");
                if ((!prov.AllowEnrollment) && (this.EnrollWizard)) 
                    throw new Exception("This Provider do not support Wizards !");
            }
        }
    }

    [Serializable]
    public class FlatOTPProvider: FlatBaseProvider
    {
        public HashMode Algorithm { get; set; }
        public int TOTPShadows { get; set; }
        public OTPWizardOptions WizardOptions { get; set; }

        /// <summary>
        /// Kind  Property
        /// </summary>
        public override PreferredMethod Kind 
        { 
            get 
            { 
                return PreferredMethod.Code; 
            } 
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            OTPProvider otp = cfg.OTPProvider;
            this.IsDirty = cfg.IsDirty;            
            this.Enabled = otp.Enabled;
            this.IsRequired = otp.IsRequired;
            this.EnrollWizard = otp.EnrollWizard;
            this.ForceWizard = otp.ForceWizard;
            this.Algorithm = otp.Algorithm;
            this.TOTPShadows = otp.TOTPShadows;
            this.WizardOptions = otp.WizardOptions;
            this.PinRequired = otp.PinRequired;
            this.FullQualifiedImplementation = otp.FullQualifiedImplementation;
            this.Parameters = otp.Parameters.Data;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            OTPProvider otp = cfg.OTPProvider;
            cfg.IsDirty = true;
            CheckUpdates(host);
            otp.Enabled = this.Enabled;
            otp.IsRequired = this.IsRequired;
            otp.EnrollWizard = this.EnrollWizard;
            otp.ForceWizard = this.ForceWizard;
            otp.Algorithm = this.Algorithm;
            otp.TOTPShadows = this.TOTPShadows;
            otp.WizardOptions = this.WizardOptions;
            otp.PinRequired = this.PinRequired;
            otp.FullQualifiedImplementation = this.FullQualifiedImplementation;
            otp.Parameters.Data = this.Parameters;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public class FlatExternalProvider: FlatBaseProvider
    {
        public string Company { get; set; }
        public string Sha1Salt { get; set; }
        public bool IsTwoWay  { get; set; }
        public int Timeout  { get; set; }

        /// <summary>
        /// Kind  Property
        /// </summary>
        public override PreferredMethod Kind
        {
            get
            {
                return PreferredMethod.External;
            }
        }


        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            ExternalOTPProvider otp = cfg.ExternalProvider;
            this.IsDirty = cfg.IsDirty;
            this.Enabled = otp.Enabled;
            this.IsRequired = otp.IsRequired;
            this.EnrollWizard = otp.EnrollWizard;
            this.ForceWizard = otp.ForceWizard;
            this.Company = otp.Company;
            this.FullQualifiedImplementation = otp.FullQualifiedImplementation;
            this.IsTwoWay = otp.IsTwoWay;
            this.Sha1Salt = otp.Sha1Salt;
            this.Timeout = otp.Timeout;
            this.PinRequired = otp.PinRequired;
            this.Parameters = otp.Parameters.Data;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            ExternalOTPProvider otp = cfg.ExternalProvider;
            cfg.IsDirty = true;
            CheckUpdates(host);
            otp.Enabled = this.Enabled;
            otp.IsRequired = this.IsRequired;
            otp.EnrollWizard = this.EnrollWizard;
            otp.ForceWizard = this.ForceWizard;
            otp.Company = this.Company;
            otp.FullQualifiedImplementation = this.FullQualifiedImplementation;
            otp.IsTwoWay = this.IsTwoWay;
            otp.Sha1Salt = this.Sha1Salt;
            otp.Timeout = this.Timeout;
            otp.Parameters.Data = this.Parameters;
            otp.PinRequired = this.PinRequired;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public class FlatAzureProvider: FlatBaseProvider
    {
        public string TenantId { get; set; }
        public string ThumbPrint { get; set; }

        /// <summary>
        /// Kind  Property
        /// </summary>
        public override PreferredMethod Kind
        {
            get
            {
                return PreferredMethod.Azure;
            }
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            AzureProvider otp = cfg.AzureProvider;
            this.IsDirty = cfg.IsDirty;
            this.Enabled = otp.Enabled;
            IsRequired = otp.IsRequired;
            this.EnrollWizard = false;
            this.ForceWizard = ForceWizardMode.Disabled;
            this.TenantId = otp.TenantId;
            this.ThumbPrint = otp.ThumbPrint;
            this.PinRequired = otp.PinRequired;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            AzureProvider otp = cfg.AzureProvider;
            cfg.IsDirty = true;
            CheckUpdates(host);
            otp.Enabled = Enabled;
            otp.EnrollWizard = false;
            otp.IsRequired = IsRequired;
            otp.ForceWizard = ForceWizardMode.Disabled;
            otp.TenantId = this.TenantId;
            otp.ThumbPrint = this.ThumbPrint;
            otp.PinRequired = this.PinRequired;
            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public class FlatBiometricProvider : FlatBaseProvider
    {
        public uint Timeout { get; set; }
        public int TimestampDriftTolerance { get; set; }
        public int ChallengeSize { get; set; }
        public string ServerDomain { get; set; }
        public string ServerName { get; set; }
        public string ServerIcon { get; set; }
        public string Origin { get; set; }
        public FlatAuthenticatorAttachmentKind AuthenticatorAttachment { get; set; }
        public FlatAttestationConveyancePreferenceKind AttestationConveyancePreference { get; set; }
        public FlatUserVerificationRequirementKind UserVerificationRequirement { get; set; }
        public bool Extensions { get; set; }
        public bool UserVerificationIndex { get; set; }
        public bool Location { get; set; }
        public bool UserVerificationMethod { get; set; }
        public bool RequireResidentKey { get; set; }

        /// <summary>
        /// Kind  Property
        /// </summary>
        public override PreferredMethod Kind
        {
            get
            {
                return PreferredMethod.Biometrics;
            }
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            WebAuthNProvider otp = cfg.WebAuthNProvider;
            this.IsDirty = cfg.IsDirty;
            this.Enabled = otp.Enabled;
            this.IsRequired = otp.IsRequired;
            this.EnrollWizard = otp.EnrollWizard;
            this.ForceWizard = otp.ForceWizard;
            this.PinRequired = otp.PinRequired;
            this.FullQualifiedImplementation = otp.FullQualifiedImplementation;
            this.Parameters = otp.Parameters.Data;

            this.Timeout = otp.Configuration.Timeout;
            this.TimestampDriftTolerance = otp.Configuration.TimestampDriftTolerance;
            this.ChallengeSize = otp.Configuration.ChallengeSize;
            this.ServerDomain = otp.Configuration.ServerDomain;
            this.ServerName = otp.Configuration.ServerName;
            this.ServerIcon = otp.Configuration.ServerIcon;
            this.Origin = otp.Configuration.Origin;
            this.AuthenticatorAttachment = otp.Options.AuthenticatorAttachment.FromAuthenticatorAttachmentValue();
            this.AttestationConveyancePreference = otp.Options.AttestationConveyancePreference.FromAttestationConveyancePreferenceValue();
            this.UserVerificationRequirement = otp.Options.UserVerificationRequirement.FromUserVerificationRequirementValue();
            this.Extensions = otp.Options.Extensions;
            this.UserVerificationIndex = otp.Options.UserVerificationIndex;
            this.Location = otp.Options.Location;
            this.UserVerificationMethod = otp.Options.UserVerificationMethod;
            this.RequireResidentKey = otp.Options.RequireResidentKey;
        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            WebAuthNProvider otp = cfg.WebAuthNProvider;
            cfg.IsDirty = true;
            CheckUpdates(host);
            otp.Enabled = Enabled;
            otp.EnrollWizard = EnrollWizard;
            otp.ForceWizard = ForceWizard;
            otp.IsRequired = IsRequired;
            otp.PinRequired = this.PinRequired;
            otp.FullQualifiedImplementation = this.FullQualifiedImplementation;
            otp.Parameters.Data = this.Parameters;

            otp.Configuration.Timeout = this.Timeout;
            otp.Configuration.TimestampDriftTolerance = this.TimestampDriftTolerance;
            otp.Configuration.ChallengeSize = this.ChallengeSize;
            otp.Configuration.ServerDomain = this.ServerDomain;
            otp.Configuration.ServerName = this.ServerName;
            otp.Configuration.ServerIcon = this.ServerIcon;
            otp.Configuration.Origin = this.Origin;
            otp.Options.AuthenticatorAttachment = this.AuthenticatorAttachment.ToAuthenticatorAttachmentValue();
            otp.Options.AttestationConveyancePreference = this.AttestationConveyancePreference.ToAttestationConveyancePreferenceValue();
            otp.Options.UserVerificationRequirement = this.UserVerificationRequirement.ToUserVerificationRequirementValue();
            otp.Options.Extensions = this.Extensions;
            otp.Options.UserVerificationIndex = this.UserVerificationIndex;
            otp.Options.Location = this.Location;
            otp.Options.UserVerificationMethod = this.UserVerificationMethod;
            otp.Options.RequireResidentKey = this.RequireResidentKey;

            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    /// <summary>
    /// FlatAuthenticatorAttachmentKind
    /// </summary>   
    public enum FlatAuthenticatorAttachmentKind
    {
        Empty = 0,
        Platform = 1,
        CrossPlatform = 2
    }

    /// <summary>
    /// FlatAttestationConveyancePreferenceKind
    /// </summary>   
    public enum FlatAttestationConveyancePreferenceKind
    {
        None = 0,
        Direct = 1,
        Indirect = 2
    }

    /// <summary>
    /// FlatUserVerificationRequirementKind
    /// </summary>      
    public enum FlatUserVerificationRequirementKind
    {
        Preferred = 0,
        Required = 1,
        Discouraged = 2
    }

    #region Extension method for convertion
    /// <summary>
    /// FlatWebAuthNExtensions
    /// </summary>   
    public static class FlatWebAuthNExtensions
    {
        /// <summary>
        /// ToAuthenticatorAttachmentValue method implementation
        /// </summary>
        public static string ToAuthenticatorAttachmentValue(this FlatAuthenticatorAttachmentKind type)
        {
            switch (type)
            {
                case FlatAuthenticatorAttachmentKind.Empty:
                    return string.Empty;
                case FlatAuthenticatorAttachmentKind.Platform:
                    return "platform";
                case FlatAuthenticatorAttachmentKind.CrossPlatform:
                    return "cross-platform";
                default:
                    return "platform";
            }
        }

        /// <summary>
        /// ToAttestationConveyancePreferenceValue method implementation
        /// </summary>
        public static string ToAttestationConveyancePreferenceValue(this FlatAttestationConveyancePreferenceKind type)
        {
            switch (type)
            {
                case FlatAttestationConveyancePreferenceKind.None:
                    return "none";
                case FlatAttestationConveyancePreferenceKind.Direct:
                    return "direct";
                case FlatAttestationConveyancePreferenceKind.Indirect:
                    return "indirect";
                default:
                    return "direct";
            }
        }

        /// <summary>
        /// ToUserVerificationRequirementValue method implementation
        /// </summary>
        public static string ToUserVerificationRequirementValue(this FlatUserVerificationRequirementKind type)
        {
            switch (type)
            {
                case FlatUserVerificationRequirementKind.Preferred:
                    return "preferred";
                case FlatUserVerificationRequirementKind.Required:
                    return "required";
                case FlatUserVerificationRequirementKind.Discouraged:
                    return "discouraged";
                default:
                    return "preferred";
            }
        }


        /// <summary>
        /// FromAuthenticatorAttachmentValue method implementation
        /// </summary>
        public static FlatAuthenticatorAttachmentKind FromAuthenticatorAttachmentValue(this string type)
        {
            switch (type.ToLower())
            {
                case "":
                    return FlatAuthenticatorAttachmentKind.Empty;
                case "platform":
                    return FlatAuthenticatorAttachmentKind.Platform;
                case "cross-platform":
                    return FlatAuthenticatorAttachmentKind.CrossPlatform;
                default:
                    return FlatAuthenticatorAttachmentKind.Platform;
            }
        }

        /// <summary>
        /// FromAttestationConveyancePreferenceValue method implementation
        /// </summary>
        public static FlatAttestationConveyancePreferenceKind FromAttestationConveyancePreferenceValue(this string type)
        {
            switch (type.ToLower())
            {
                case "none":
                    return FlatAttestationConveyancePreferenceKind.None;
                case "direct":
                    return FlatAttestationConveyancePreferenceKind.Direct;
                case "indirect":
                    return FlatAttestationConveyancePreferenceKind.Indirect;
                default:
                    return FlatAttestationConveyancePreferenceKind.Direct;
            }
        }

        /// <summary>
        /// FromAttestationConveyancePreferenceValue method implementation
        /// </summary>
        public static FlatUserVerificationRequirementKind FromUserVerificationRequirementValue(this string type)
        {
            switch (type.ToLower())
            {
                case "preferred":
                    return FlatUserVerificationRequirementKind.Preferred;
                case "required":
                    return FlatUserVerificationRequirementKind.Required;
                case "discouraged":
                    return FlatUserVerificationRequirementKind.Discouraged;
                default:
                    return FlatUserVerificationRequirementKind.Preferred;
            }
        }
    }
    #endregion

    [Serializable]
    public class FlatConfigMail: FlatBaseProvider
    {
        public string From { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string Company { get; set; }
        public bool Anonymous { get; set; }
        public bool DeliveryNotifications { get; set; }
        public FlatConfigMailAllowedDomains AllowedDomains { get; set; }
        public FlatConfigMailBlockedDomains BlockedDomains { get; set; }
        public List<FlatConfigMailFileName> MailOTPContent { get; set; }
        public List<FlatConfigMailFileName> MailAdminContent { get; set; }
        public List<FlatConfigMailFileName> MailKeyContent { get; set; }
        public List<FlatConfigMailFileName> MailNotifications { get; set; }

        public FlatConfigMail()
        {
            this.MailOTPContent = new List<FlatConfigMailFileName>();
            this.MailAdminContent = new List<FlatConfigMailFileName>();
            this.MailKeyContent = new List<FlatConfigMailFileName>();
            this.MailNotifications = new List<FlatConfigMailFileName>();
            this.BlockedDomains = new FlatConfigMailBlockedDomains();
            this.AllowedDomains = new FlatConfigMailAllowedDomains();
        }

        /// <summary>
        /// Kind  Property
        /// </summary>
        public override PreferredMethod Kind
        {
            get
            {
                return PreferredMethod.Email;
            }
        }


        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Load(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            MailProvider mail = cfg.MailProvider;
            IsDirty = cfg.IsDirty;
            Enabled = mail.Enabled;
            EnrollWizard = mail.EnrollWizard;
            ForceWizard = mail.ForceWizard;
            From = mail.From;
            UserName = mail.UserName;
            Password = mail.Password;
            Host = mail.Host;
            Port = mail.Port;
            UseSSL = mail.UseSSL;
            Company = mail.Company;
            PinRequired = mail.PinRequired;
            Anonymous = mail.Anonymous;
            DeliveryNotifications = mail.DeliveryNotifications;
            FullQualifiedImplementation = mail.FullQualifiedImplementation;
            Parameters = mail.Parameters.Data;

            AllowedDomains.Clear();
            foreach (string itm in mail.AllowedDomains)
            {
                AllowedDomains.AddDomain(itm);
            }

            BlockedDomains.Clear();
            foreach (string itm in mail.BlockedDomains)
            {
                BlockedDomains.AddDomain(itm);
            }

            MailOTPContent.Clear();
            foreach (SendMailFileName itm in mail.MailOTPContent)
            {
                MailOTPContent.Add((FlatConfigMailFileName)itm);
            }
            MailAdminContent.Clear();
            foreach (SendMailFileName itm in mail.MailAdminContent)
            {
                MailAdminContent.Add((FlatConfigMailFileName)itm);
            }
            MailKeyContent.Clear();
            foreach (SendMailFileName itm in mail.MailKeyContent)
            {
                MailKeyContent.Add((FlatConfigMailFileName)itm);
            }
            MailNotifications.Clear();
            foreach (SendMailFileName itm in mail.MailNotifications)
            {
                MailNotifications.Add((FlatConfigMailFileName)itm);
            }

        }

        /// <summary>
        /// Update method implmentation
        /// </summary>
        public override void Update(PSHost host)
        {
            ManagementService.Initialize(host, true);
            MFAConfig cfg = ManagementService.Config;
            MailProvider mail = cfg.MailProvider;
            cfg.IsDirty = true;
            CheckUpdates(host);
            mail.Enabled = Enabled;
            mail.EnrollWizard = EnrollWizard;
            mail.ForceWizard = ForceWizard;
            mail.From = From;
            mail.UserName = UserName;
            mail.Password = Password;
            mail.Host = Host;
            mail.Port = Port;
            mail.UseSSL = UseSSL;
            mail.Company = Company;
            mail.PinRequired = PinRequired;
            mail.Anonymous = Anonymous;
            mail.DeliveryNotifications = DeliveryNotifications;
            mail.FullQualifiedImplementation = FullQualifiedImplementation;
            mail.Parameters.Data = Parameters;

            mail.AllowedDomains.Clear();
            foreach (string itm in AllowedDomains.Domains)
            {
                mail.AllowedDomains.Add(itm);
            }

            mail.BlockedDomains.Clear();
            foreach (string itm in BlockedDomains.Domains)
            {
                mail.BlockedDomains.Add(itm);
            }

            mail.MailOTPContent.Clear();
            foreach (FlatConfigMailFileName itm in MailOTPContent)
            {
                mail.MailOTPContent.Add((SendMailFileName)itm);
            }
            mail.MailAdminContent.Clear();
            foreach (FlatConfigMailFileName itm in MailAdminContent)
            {
                mail.MailAdminContent.Add((SendMailFileName)itm);
            }
            mail.MailKeyContent.Clear();
            foreach (FlatConfigMailFileName itm in MailKeyContent)
            {
                mail.MailKeyContent.Add((SendMailFileName)itm);
            }
            mail.MailNotifications.Clear();
            foreach (FlatConfigMailFileName itm in MailNotifications)
            {
                mail.MailNotifications.Add((SendMailFileName)itm);
            }

            ManagementService.ADFSManager.WriteConfiguration(host);
        }
    }

    [Serializable]
    public class FlatConfigMailBlockedDomains
    {
        private List<string> _list = new List<string>();

        /// <summary>
        /// <para type="description">BlockedDomains property.</para>
        /// </summary>
        public List<string> Domains
        {
            get
            {
                return _list;
            }
        }

        /// <summary>
        /// Clear method implmentation
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }
        
        /// <summary>
        /// AddDomain method implmentation
        /// </summary>
        public void AddDomain(string domainname)
        {
            try
            {
                if (_list.Contains(domainname.ToLower()))
                    throw new Exception("Domain is already int blocked list !");
                _list.Add(domainname.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception("Error Adding blocked domain !", ex);
            }
        }

        /// <summary>
        /// RemoveDomain method implmentation
        /// </summary>
        public void RemoveDomain(string domainname)
        {
            try
            {
                if (!_list.Contains(domainname.ToLower()))
                    throw new Exception("Domain not found int blocked list !");
                int i = _list.IndexOf(domainname.ToLower());
                _list.RemoveAt(i);
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing blocked domain !", ex);
            }
        }
    }

    [Serializable]
    public class FlatConfigMailAllowedDomains
    {
        private List<string> _list = new List<string>();

        /// <summary>
        /// <para type="description">AllowedDomains property.</para>
        /// </summary>
        public List<string> Domains
        {
            get
            {
                return _list;
            }
        }

        /// <summary>
        /// Clear method implmentation
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// AddDomain method implmentation
        /// </summary>
        public void AddDomain(string domainname)
        {
            try
            {
                if (_list.Contains(domainname.ToLower()))
                    throw new Exception("Domain is already int blocked list !");
                _list.Add(domainname.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception("Error Adding blocked domain !", ex);
            }
        }

        /// <summary>
        /// RemoveDomain method implmentation
        /// </summary>
        public void RemoveDomain(string domainname)
        {
            try
            {
                if (!_list.Contains(domainname.ToLower()))
                    throw new Exception("Domain not found int blocked list !");
                int i = _list.IndexOf(domainname.ToLower());
                _list.RemoveAt(i);
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing blocked domain !", ex);
            }
        }
    }

    [Serializable]
    public class FlatConfigMailFileName
    {
        public int LCID { get; set; }
        public string FileName { get; set; }
        public bool Enabled { get; set; }

        public FlatConfigMailFileName(int lcid, string filename, bool enabled = true)
        {
            this.LCID = lcid;
            this.FileName = filename;
            this.Enabled = enabled;
        }

        public static explicit operator SendMailFileName(FlatConfigMailFileName file)
        {
            if (file == null)
                return null;
            else
                return new SendMailFileName(file.LCID, file.FileName, file.Enabled);
        }

        public static explicit operator FlatConfigMailFileName(SendMailFileName file)
        {
            if (file == null)
                return null;
            else
                return new FlatConfigMailFileName(file.LCID, file.FileName, file.Enabled);
        }
    }

    [Serializable]
    public class FlatConfigAdvertising
    {
        public uint FirstDay { get; set; }
        public uint LastDay { get; set; }
        public bool OnFire { get; set; }

        public static explicit operator ConfigAdvertising(FlatConfigAdvertising adv)
        {
            ConfigAdvertising cfg = new ConfigAdvertising
            {
                FirstDay = adv.FirstDay,
                LastDay = adv.LastDay
            };
            return cfg;
        }

        public static explicit operator FlatConfigAdvertising(ConfigAdvertising adv)
        {
            FlatConfigAdvertising cfg = new FlatConfigAdvertising
            {
                FirstDay = adv.FirstDay,
                LastDay = adv.LastDay,
                OnFire = adv.OnFire
            };
            return cfg;
        }
    }
}
