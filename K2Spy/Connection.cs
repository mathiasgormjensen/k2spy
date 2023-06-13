using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace K2Spy
{
    public class Connection
    {
        #region Constructors

        public Connection()
        {
        }

        public Connection(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region Public Properties

        [XmlIgnore]
        public string ShortDisplayName
        {
            get { return this.BuildDisplayName(false); }
        }

        [XmlIgnore]
        public string FullDisplayName
        {
            get { return this.BuildDisplayName(true); }
        }

#if false
        public string ConnectionString { get; set; }
#else
        [XmlIgnore]
        public string ConnectionString
        {
            get { return this.Decrypt(this.EncryptedConnectionString); }
            set { this.EncryptedConnectionString = this.Encrypt(value); }
        }

        public string EncryptedConnectionString { get; set; }
#endif

        public bool Selected { get; set; }

        #endregion

        #region Protected Methods

        protected string BuildDisplayName(bool full)
        {
            StringBuilder builder = new StringBuilder();
            SCConnectionStringBuilder sCConnectionStringBuilder = new SCConnectionStringBuilder(this.ConnectionString ?? "");
            if (!sCConnectionStringBuilder.Integrated)
                builder.Append($"{sCConnectionStringBuilder.UserID}@");
            builder.Append(sCConnectionStringBuilder.Host);
            builder.Append(":");
            builder.Append(sCConnectionStringBuilder.Port);

            if (full)
            {
                builder.Append(" - ");
                if (!string.IsNullOrEmpty(sCConnectionStringBuilder.Password))
                    sCConnectionStringBuilder.Password = "*****";
                builder.Append(sCConnectionStringBuilder.ConnectionString);
            }
            return builder.ToString();
        }

        protected SCConnectionStringBuilder GetSCConnectionStringBuilder()
        {
            return new SCConnectionStringBuilder(this.ConnectionString);
        }

        protected string Encrypt(string value)
        {
            byte[] decryptedBytes = Encoding.UTF8.GetBytes(value);
            byte[] encryptedBytes = ProtectedData.Protect(decryptedBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedBytes);
        }

        protected string Decrypt(string value)
        {
            byte[] encryptedBytes = Convert.FromBase64String(value);
            byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        #endregion
    }
}