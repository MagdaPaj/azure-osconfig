// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace E2eTesting
{
    [TestFixture, Category("Firewall")]
    public class FirewallTests : E2ETest
    {
        protected static string _componentName = "Firewall";

        private static Regex _statePattern = new Regex(@"[0-2]");
        private static Regex _fingerprintPattern = new Regex(@"[0-9a-z]{64}");

        public enum FirewallStateCode
        {
            Unknown = 0,
            Enabled,
            Disabled,
        }

        public partial class Firewall
        {
            public FirewallStateCode firewallState { get; set; }
            public string firewallFingerprint { get; set; }
        }

        [Test]
        public async Task FirewallTest_Regex()
        {
            Firewall reported = await GetReported<Firewall>(_componentName, (Firewall firewall) => (firewall.firewallState != FirewallStateCode.Unknown) && (firewall.firewallFingerprint != null));

            Assert.Multiple(() =>
            {
                RegexAssert.IsMatch(_statePattern, reported.firewallState);
                RegexAssert.IsMatch(_fingerprintPattern, reported.firewallFingerprint);
            });
        }
    }
}