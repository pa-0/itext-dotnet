using System;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Utils;
using Org.BouncyCastle.Cert;

namespace iText.Bouncycastlefips.Cert {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Cert.X509Crl"/>.
    /// </summary>
    public class X509CrlBCFips : IX509Crl {
        private readonly X509Crl x509Crl;

        /// <summary>
        /// Creates new wrapper instance for <see cref="Org.BouncyCastle.Cert.X509Crl"/>.
        /// </summary>
        /// <param name="x509Crl">
        /// <see cref="Org.BouncyCastle.Cert.X509Crl"/>
        /// to be wrapped
        /// </param>
        public X509CrlBCFips(X509Crl x509Crl) {
            this.x509Crl = x509Crl;
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped <see cref="Org.BouncyCastle.Cert.X509Crl"/>.
        /// </returns>
        public virtual X509Crl GetX509Crl() {
            return x509Crl;
        }

        /// <summary><inheritDoc/></summary>
        public bool IsRevoked(IX509Certificate cert) {
            return x509Crl.IsRevoked(((X509CertificateBCFips)cert).GetCertificate());
        }

        /// <summary>Indicates whether some other object is "equal to" this one. Compares wrapped objects.</summary>
        public override bool Equals(Object o) {
            if (this == o) {
                return true;
            }
            if (o == null || GetType() != o.GetType()) {
                return false;
            }
            X509CrlBCFips that = (X509CrlBCFips)o;
            return Object.Equals(x509Crl, that.x509Crl);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode(x509Crl);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return x509Crl.ToString();
        }
    }
}