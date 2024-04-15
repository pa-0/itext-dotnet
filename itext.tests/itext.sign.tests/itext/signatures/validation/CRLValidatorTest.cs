/*
This file is part of the iText (R) project.
Copyright (c) 1998-2024 Apryse Group NV
Authors: Apryse Software.

This program is offered under a commercial and under the AGPL license.
For commercial licensing, contact us at https://itextpdf.com/sales.  For AGPL licensing, see below.

AGPL licensing:
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Signatures;
using iText.Signatures.Logs;
using iText.Signatures.Testutils;
using iText.Signatures.Testutils.Builder;
using iText.Signatures.Validation.Extensions;
using iText.Signatures.Validation.Report;
using iText.Test;

namespace iText.Signatures.Validation {
    [NUnit.Framework.Category("BouncyCastleUnitTest")]
    public class CRLValidatorTest : ExtendedITextTest {
        private static readonly String SOURCE_FOLDER = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/signatures/validation/CRLValidatorTest/";

        private static readonly IBouncyCastleFactory FACTORY = BouncyCastleFactoryCreator.GetFactory();

        private static readonly char[] KEY_PASSWORD = "testpassphrase".ToCharArray();

        private CRLValidator validator;

        private CRLValidatorTest.MockChainValidator mockChainValidator;

        private IX509Certificate crlIssuerCert;

        private IX509Certificate signCert;

        private IPrivateKey crlIssuerKey;

        private IPrivateKey intermediateKey;

        [NUnit.Framework.OneTimeSetUp]
        public static void SetUpOnce() {
        }

        [NUnit.Framework.SetUp]
        public virtual void SetUp() {
            validator = new CRLValidator();
            mockChainValidator = new CRLValidatorTest.MockChainValidator();
            validator.SetCertificateChainValidator(mockChainValidator);
        }

        [NUnit.Framework.Test]
        public virtual void HappyPathTest() {
            RetrieveTestResources("happyPath");
            byte[] crl = CreateCrl(crlIssuerCert, crlIssuerKey, TimeTestUtil.TEST_DATE_TIME.AddDays(-5), TimeTestUtil.
                TEST_DATE_TIME.AddDays(+5));
            ValidationReport report = PerformValidation("happyPath", TimeTestUtil.TEST_DATE_TIME, crl);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.VALID, report.GetValidationResult());
            NUnit.Framework.Assert.IsTrue(report.GetFailures().IsEmpty());
        }

        [NUnit.Framework.Test]
        public virtual void NextUpdateBeforeValidationTest() {
            RetrieveTestResources("happyPath");
            DateTime nextUpdate = TimeTestUtil.TEST_DATE_TIME.AddDays(-5);
            byte[] crl = CreateCrl(crlIssuerCert, crlIssuerKey, TimeTestUtil.TEST_DATE_TIME.AddDays(-15), nextUpdate);
            ValidationReport report = PerformValidation("happyPath", TimeTestUtil.TEST_DATE_TIME, crl);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format(CRLValidator.UPDATE_DATE_BEFORE_CHECK_DATE, nextUpdate
                , TimeTestUtil.TEST_DATE_TIME), report.GetFailures()[0].GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void ChainValidatorUsageTest() {
            RetrieveTestResources("happyPath");
            byte[] crl = CreateCrl(crlIssuerCert, crlIssuerKey, TimeTestUtil.TEST_DATE_TIME.AddDays(-5), TimeTestUtil.
                TEST_DATE_TIME.AddDays(+5));
            ValidationReport report = PerformValidation("happyPath", TimeTestUtil.TEST_DATE_TIME, crl);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.VALID, report.GetValidationResult());
            NUnit.Framework.Assert.IsTrue(report.GetFailures().IsEmpty());
            NUnit.Framework.Assert.AreEqual(1, mockChainValidator.verificationCalls.Count);
            NUnit.Framework.Assert.AreEqual(1, mockChainValidator.verificationCalls[0].requiredExtensions.Count);
            NUnit.Framework.Assert.AreEqual(new KeyUsageExtension(KeyUsage.CRL_SIGN), mockChainValidator.verificationCalls
                [0].requiredExtensions[0]);
            NUnit.Framework.Assert.AreEqual(crlIssuerCert, mockChainValidator.verificationCalls[0].certificate);
        }

        [NUnit.Framework.Test]
        public virtual void IssuerCertificateIsNotFoundTest() {
            RetrieveTestResources("missingIssuer");
            byte[] crl = CreateCrl(crlIssuerCert, crlIssuerKey, TimeTestUtil.TEST_DATE_TIME.AddDays(-5), TimeTestUtil.
                TEST_DATE_TIME.AddDays(+5));
            ValidationReport report = PerformValidation("missingIssuer", TimeTestUtil.TEST_DATE_TIME, crl);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(CRLValidator.CRL_ISSUER_NOT_FOUND, report.GetFailures()[0].GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void CrlIssuerAndSignCertHaveNoSharedRootTest() {
            RetrieveTestResources("crlIssuerAndSignCertHaveNoSharedRoot");
            byte[] crl = CreateCrl(crlIssuerCert, crlIssuerKey, TimeTestUtil.TEST_DATE_TIME.AddDays(-5), TimeTestUtil.
                TEST_DATE_TIME.AddDays(+5));
            ValidationReport report = PerformValidation("crlIssuerAndSignCertHaveNoSharedRoot", TimeTestUtil.TEST_DATE_TIME
                , crl);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(CRLValidator.CRL_ISSUER_NO_COMMON_ROOT, report.GetFailures()[0].GetMessage
                ());
        }

        [NUnit.Framework.Test]
        public virtual void CrlIssuerRevokedBeforeSigningDate() {
            // CRL has the certificate revoked before signing date
            RetrieveTestResources("crlIssuerRevokedBeforeSigningDate");
            DateTime revocationDate = TimeTestUtil.TEST_DATE_TIME.AddDays(-2);
            byte[] crl = CreateCrl(crlIssuerCert, crlIssuerKey, TimeTestUtil.TEST_DATE_TIME.AddDays(-5), TimeTestUtil.
                TEST_DATE_TIME.AddDays(+5), signCert, revocationDate, 1);
            ValidationReport report = PerformValidation("crlIssuerRevokedBeforeSigningDate", TimeTestUtil.TEST_DATE_TIME
                , crl);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INVALID, report.GetValidationResult());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format(CRLValidator.CERTIFICATE_REVOKED, crlIssuerCert.GetSubjectDN
                (), revocationDate), report.GetFailures()[0].GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void CrlRevokedAfterSigningDate() {
            // CRL has the certificate revoked after signing date
            RetrieveTestResources("happyPath");
            DateTime revocationDate = TimeTestUtil.TEST_DATE_TIME.AddDays(+20);
            byte[] crl = CreateCrl(crlIssuerCert, crlIssuerKey, TimeTestUtil.TEST_DATE_TIME.AddDays(+18), TimeTestUtil
                .TEST_DATE_TIME.AddDays(+23), signCert, revocationDate, 1);
            ValidationReport report = PerformValidation("happyPath", TimeTestUtil.TEST_DATE_TIME, crl);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.VALID, report.GetValidationResult());
            NUnit.Framework.Assert.AreEqual(2, report.GetLogs().Count);
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format(SignLogMessageConstant.VALID_CERTIFICATE_IS_REVOKED
                , revocationDate), report.GetLogs()[1].GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void CrlSignatureMismatch() {
            //CRL response is invalid (signature not matching)
            RetrieveTestResources("happyPath");
            byte[] crl = CreateCrl(crlIssuerCert, intermediateKey, TimeTestUtil.TEST_DATE_TIME.AddDays(+18), TimeTestUtil
                .TEST_DATE_TIME.AddDays(+23), signCert, TimeTestUtil.TEST_DATE_TIME.AddDays(+20), 1);
            ValidationReport report = PerformValidation("happyPath", TimeTestUtil.TEST_DATE_TIME, crl);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(CRLValidator.CRL_INVALID, report.GetFailures()[0].GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void CrlContainsOnlyCACertsTest() {
            String crlPath = SOURCE_FOLDER + "issuingDistributionPointTest/onlyCA.crl";
            ValidationReport report = CheckCrlScope(crlPath);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(CRLValidator.CERTIFICATE_IS_NOT_IN_THE_CRL_SCOPE, report.GetFailures()[0].
                GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void CrlContainsOnlyUserCertsTest() {
            String crlPath = SOURCE_FOLDER + "issuingDistributionPointTest/onlyUser.crl";
            ValidationReport report = CheckCrlScope(crlPath);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.VALID, report.GetValidationResult());
            NUnit.Framework.Assert.AreEqual(0, report.GetFailures().Count);
        }

        [NUnit.Framework.Test]
        public virtual void CrlContainsOnlyAttributeCertsTest() {
            String crlPath = SOURCE_FOLDER + "issuingDistributionPointTest/onlyAttr.crl";
            ValidationReport report = CheckCrlScope(crlPath);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(CRLValidator.ATTRIBUTE_CERTS_ASSERTED, report.GetFailures()[0].GetMessage(
                ));
        }

        [NUnit.Framework.Test]
        public virtual void OnlySomeReasonsTest() {
            String root = SOURCE_FOLDER + "issuingDistributionPointTest/root.pem";
            String sign = SOURCE_FOLDER + "issuingDistributionPointTest/sign.pem";
            IX509Certificate rootCert = (IX509Certificate)PemFileHelper.ReadFirstChain(root)[0];
            IPrivateKey rootKey = PemFileHelper.ReadFirstKey(root, KEY_PASSWORD);
            IX509Certificate signCert = (IX509Certificate)PemFileHelper.ReadFirstChain(sign)[0];
            TestCrlBuilder builder = new TestCrlBuilder(rootCert, rootKey);
            builder.AddExtension(FACTORY.CreateExtensions().GetIssuingDistributionPoint(), true, FACTORY.CreateIssuingDistributionPoint
                (null, false, false, FACTORY.CreateReasonFlags(CRLValidator.ALL_REASONS - 31), false, false));
            IssuingCertificateRetriever certificateRetriever = new IssuingCertificateRetriever();
            certificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            ValidationReport report = new ValidationReport();
            validator.SetIssuingCertificateRetriever(certificateRetriever);
            validator.Validate(report, signCert, (IX509Crl)CertificateUtil.ParseCrlFromStream(new MemoryStream(builder
                .MakeCrl())), TimeTestUtil.TEST_DATE_TIME);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            CertificateReportItem reportItem = (CertificateReportItem)report.GetFailures()[0];
            NUnit.Framework.Assert.AreEqual(signCert, reportItem.GetCertificate());
            NUnit.Framework.Assert.AreEqual(CRLValidator.ONLY_SOME_REASONS_CHECKED, reportItem.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void CheckLessReasonsTest() {
            String fullCrlPath = SOURCE_FOLDER + "issuingDistributionPointTest/onlyUser.crl";
            String root = SOURCE_FOLDER + "issuingDistributionPointTest/root.pem";
            String sign = SOURCE_FOLDER + "issuingDistributionPointTest/sign.pem";
            IX509Certificate rootCert = (IX509Certificate)PemFileHelper.ReadFirstChain(root)[0];
            IPrivateKey rootKey = PemFileHelper.ReadFirstKey(root, KEY_PASSWORD);
            IX509Certificate signCert = (IX509Certificate)PemFileHelper.ReadFirstChain(sign)[0];
            TestCrlBuilder builder = new TestCrlBuilder(rootCert, rootKey);
            builder.AddExtension(FACTORY.CreateExtensions().GetIssuingDistributionPoint(), true, FACTORY.CreateIssuingDistributionPoint
                (null, false, false, FACTORY.CreateReasonFlags(CRLValidator.ALL_REASONS - 31), false, false));
            IssuingCertificateRetriever certificateRetriever = new IssuingCertificateRetriever();
            certificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            ValidationReport report = new ValidationReport();
            validator.SetIssuingCertificateRetriever(certificateRetriever);
            // Validate full CRL.
            validator.Validate(report, signCert, (IX509Crl)CertificateUtil.ParseCrlFromStream(FileUtil.GetInputStreamForFile
                (fullCrlPath)), TimeTestUtil.TEST_DATE_TIME);
            // Validate CRL with onlySomeReasons.
            validator.Validate(report, signCert, (IX509Crl)CertificateUtil.ParseCrlFromStream(new MemoryStream(builder
                .MakeCrl())), TimeTestUtil.TEST_DATE_TIME);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.VALID, report.GetValidationResult());
            NUnit.Framework.Assert.AreEqual(0, report.GetFailures().Count);
            CertificateReportItem reportItem = (CertificateReportItem)report.GetLogs()[1];
            NUnit.Framework.Assert.AreEqual(signCert, reportItem.GetCertificate());
            NUnit.Framework.Assert.AreEqual(CRLValidator.SAME_REASONS_CHECK, reportItem.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void RemoveFromCrlTest() {
            String root = SOURCE_FOLDER + "issuingDistributionPointTest/root.pem";
            String sign = SOURCE_FOLDER + "issuingDistributionPointTest/sign.pem";
            IX509Certificate rootCert = (IX509Certificate)PemFileHelper.ReadFirstChain(root)[0];
            IPrivateKey rootKey = PemFileHelper.ReadFirstKey(root, KEY_PASSWORD);
            IX509Certificate signCert = (IX509Certificate)PemFileHelper.ReadFirstChain(sign)[0];
            TestCrlBuilder builder = new TestCrlBuilder(rootCert, rootKey);
            builder.AddCrlEntry(signCert, TimeTestUtil.TEST_DATE_TIME.AddDays(-1), FACTORY.CreateCRLReason().GetRemoveFromCRL
                ());
            IssuingCertificateRetriever certificateRetriever = new IssuingCertificateRetriever();
            certificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            ValidationReport report = new ValidationReport();
            validator.SetIssuingCertificateRetriever(certificateRetriever);
            validator.Validate(report, signCert, (IX509Crl)CertificateUtil.ParseCrlFromStream(new MemoryStream(builder
                .MakeCrl())), TimeTestUtil.TEST_DATE_TIME);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.VALID, report.GetValidationResult());
            NUnit.Framework.Assert.AreEqual(0, report.GetFailures().Count);
            CertificateReportItem reportItem = (CertificateReportItem)report.GetLogs()[1];
            NUnit.Framework.Assert.AreEqual(signCert, reportItem.GetCertificate());
            NUnit.Framework.Assert.AreEqual(CRLValidator.CERTIFICATE_IS_UNREVOKED, reportItem.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void FullCrlButDistributionPointWithReasonsTest() {
            DateTime checkDate = TimeTestUtil.TEST_DATE_TIME;
            IX509Certificate caCert = (IX509Certificate)PemFileHelper.ReadFirstChain(SOURCE_FOLDER + "issuingDistributionPointTest/rootCert.pem"
                )[0];
            IPrivateKey caPrivateKey = PemFileHelper.ReadFirstKey(SOURCE_FOLDER + "issuingDistributionPointTest/rootCert.pem"
                , KEY_PASSWORD);
            IX509Certificate cert = (IX509Certificate)PemFileHelper.ReadFirstChain(SOURCE_FOLDER + "issuingDistributionPointTest/certWithDPReasons.pem"
                )[0];
            TestCrlBuilder builder = new TestCrlBuilder(caCert, caPrivateKey);
            builder.AddExtension(FACTORY.CreateExtensions().GetIssuingDistributionPoint(), true, FACTORY.CreateIssuingDistributionPoint
                (FACTORY.CreateDistributionPointName(FACTORY.CreateCRLDistPoint(CertificateUtil.GetExtensionValue(cert
                , FACTORY.CreateExtensions().GetCRlDistributionPoints().GetId())).GetDistributionPoints()[0].GetCRLIssuer
                ()), false, false, null, false, false));
            IssuingCertificateRetriever certificateRetriever = new IssuingCertificateRetriever();
            certificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(caCert));
            ValidationReport report = new ValidationReport();
            validator.SetIssuingCertificateRetriever(certificateRetriever);
            validator.Validate(report, cert, (IX509Crl)CertificateUtil.ParseCrlFromStream(new MemoryStream(builder.MakeCrl
                ())), checkDate);
            NUnit.Framework.Assert.AreEqual(ValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            CertificateReportItem reportItem = (CertificateReportItem)report.GetLogs()[1];
            NUnit.Framework.Assert.AreEqual(ReportItem.ReportItemStatus.INDETERMINATE, reportItem.GetStatus());
            NUnit.Framework.Assert.AreEqual(cert, reportItem.GetCertificate());
            NUnit.Framework.Assert.AreEqual(CRLValidator.ONLY_SOME_REASONS_CHECKED, reportItem.GetMessage());
        }

        private ValidationReport CheckCrlScope(String crlPath) {
            String root = SOURCE_FOLDER + "issuingDistributionPointTest/root.pem";
            String sign = SOURCE_FOLDER + "issuingDistributionPointTest/sign.pem";
            IX509Certificate rootCert = (IX509Certificate)PemFileHelper.ReadFirstChain(root)[0];
            IX509Certificate signCert = (IX509Certificate)PemFileHelper.ReadFirstChain(sign)[0];
            IssuingCertificateRetriever certificateRetriever = new IssuingCertificateRetriever();
            certificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            ValidationReport report = new ValidationReport();
            validator.SetIssuingCertificateRetriever(certificateRetriever);
            validator.Validate(report, signCert, (IX509Crl)CertificateUtil.ParseCrlFromStream(FileUtil.GetInputStreamForFile
                (crlPath)), TimeTestUtil.TEST_DATE_TIME);
            return report;
        }

        private void RetrieveTestResources(String path) {
            String resourcePath = SOURCE_FOLDER + path + "/";
            crlIssuerCert = (IX509Certificate)PemFileHelper.ReadFirstChain(resourcePath + "crl-issuer.cert.pem")[0];
            signCert = (IX509Certificate)PemFileHelper.ReadFirstChain(resourcePath + "sign.cert.pem")[0];
            crlIssuerKey = PemFileHelper.ReadFirstKey(SOURCE_FOLDER + "keys/crl-key.pem", KEY_PASSWORD);
            intermediateKey = PemFileHelper.ReadFirstKey(SOURCE_FOLDER + "keys/im_key.pem", KEY_PASSWORD);
        }

        private byte[] CreateCrl(IX509Certificate issuerCert, IPrivateKey issuerKey, DateTime issueDate, DateTime 
            nextUpdate) {
            return CreateCrl(issuerCert, issuerKey, issueDate, nextUpdate, null, (DateTime)TimestampConstants.UNDEFINED_TIMESTAMP_DATE
                , 0);
        }

        private byte[] CreateCrl(IX509Certificate issuerCert, IPrivateKey issuerKey, DateTime issueDate, DateTime 
            nextUpdate, IX509Certificate revokedCert, DateTime revocationDate, int reason) {
            TestCrlBuilder builder = new TestCrlBuilder(issuerCert, issuerKey);
            if (nextUpdate != null) {
                builder.SetNextUpdate(nextUpdate);
            }
            if (revocationDate != TimestampConstants.UNDEFINED_TIMESTAMP_DATE && revokedCert != null) {
                builder.AddCrlEntry(revokedCert, revocationDate, reason);
            }
            return builder.MakeCrl();
        }

        public virtual ValidationReport PerformValidation(String testName, DateTime testDate, byte[] encodedCrl) {
            String resourcePath = SOURCE_FOLDER + testName + '/';
            String missingCertsFileName = resourcePath + "chain.pem";
            IX509Certificate[] knownCerts = PemFileHelper.ReadFirstChain(missingCertsFileName);
            IssuingCertificateRetriever mockCertificateRetriever = new IssuingCertificateRetriever();
            mockCertificateRetriever.AddKnownCertificates(JavaUtil.ArraysAsList(knownCerts));
            validator.SetIssuingCertificateRetriever(mockCertificateRetriever);
            IX509Certificate certificateUnderTest = (IX509Certificate)PemFileHelper.ReadFirstChain(resourcePath + "sign.cert.pem"
                )[0];
            ValidationReport result = new ValidationReport();
            validator.Validate(result, certificateUnderTest, (IX509Crl)CertificateUtil.ParseCrlFromStream(new MemoryStream
                (encodedCrl)), testDate);
            return result;
        }

        private class MockChainValidator : CertificateChainValidator {
            public IList<CRLValidatorTest.ValidationCallBack> verificationCalls = new List<CRLValidatorTest.ValidationCallBack
                >();

            public override ValidationReport Validate(ValidationReport result, IX509Certificate certificate, DateTime 
                verificationDate, IList<CertificateExtension> requiredExtensions) {
                verificationCalls.Add(new CRLValidatorTest.ValidationCallBack(certificate, verificationDate, requiredExtensions
                    ));
                return result;
            }
        }

        private class ValidationCallBack {
            public IX509Certificate certificate;

            public DateTime checkDate;

            public IList<CertificateExtension> requiredExtensions;

            public ValidationCallBack(IX509Certificate certificate, DateTime checkDate, IList<CertificateExtension> requiredExtensions
                ) {
                this.certificate = certificate;
                this.checkDate = checkDate;
                this.requiredExtensions = requiredExtensions;
            }
        }
    }
}