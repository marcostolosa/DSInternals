﻿using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DSInternals.Common.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSInternals.Common.Test
{
    [TestClass]
    public class KeyCredentialTester
    {
        private const string DummyDN = "CN=Account,DC=contoso,DC=com";

        [TestMethod]
        public void KeyCredential_Parse_NonMFAKey()
        {
            // Dummy key incorrectly used by the official ADComputerKeys module to "delete" existing values from registered devices.
            byte[] blob = "000200002000010D76D33954251DA969022D0D3B009939E256A6C9B3FF657907C72063F89AE79E200002F6B00E6A9BA3066ABDE0E4B23EB82D5E42898263AD46CA84BE0CFD20E81F91C00E01033082010A0282010100D6589A6FE210490583C1DCD57E3579AB24979D9B1A7118E3553DEDCFFA5CF5ABD41CF6C19CBBE598CE6F9140541E8FF8A778BD5CAADD8D038A49785A4D9031C98E26783E824BA3CF00D86C112A9A5C65A5ACF2B077E365D947BD41A437E7034CC00A77550B2EA8CEC18C1F7516DA4DC13177E1DE1D32FBBDDE1E1FD7395AAB71A8F302B985A64248C3A239E6943AEAFA9A8B591AE499F31723F7DC8A22A6D197445056DA4DF9D13443DB4A6201D52D82795A2F2FFA2F75B6F2605E213609A39DF33F26E023D83D9C4BDDD4879E234407833BA38460CBC66D9D31CDF2C5B3A042F321DA7F2140ECC4A5A190306ED51FE0EA5273DD83D5338B2554ABD3738A06A50203010001010004010100050002000701020800086254F138261CD3010800096254F138261CD301".HexToBinary();
            var key = new KeyCredential(blob, DummyDN);
            Assert.AreEqual(KeyCredentialVersion.Version2, key.Version);
            Assert.AreEqual(KeyUsage.NGC, key.Usage);
            Assert.AreEqual(KeySource.ActiveDirectory, key.Source);
            Assert.IsTrue(key.CustomKeyInfo.Flags.HasFlag(KeyFlags.MFANotUsed));

            // Serialize
            byte[] serialized = key.ToByteArray();
            Assert.AreEqual(blob.Length, serialized.Length);
            CollectionAssert.AreEqual(blob, serialized);
        }

        [TestMethod]
        public void KeyCredential_Parse_UserKey1()
        {
            byte[] blob = "0002000020000120717AE052FCCF546AAD0D51E878AAD69CE04FDC39F5A8D8E3CEBA6BCB4DA0E720000214B7474E61C1001D3E546CFED8E387CFC1AC86A2CA7B3CDCF1267614585E2A341B0103525341310008000003000000000100000000000000000000010001C1A78914457758B0B13C70C710C7F8548F3F9ED56AD4640B6E6A112655C98ECAC1CBD68A298F5686C08439428A97FE6FDF58D78EA481905182BAD684C2D9C5CDE1CDE34AA19742E8BBF58B953EAC4C562FCF598CC176B02DBE9FFFEF5937A65815C236F92892F7E511A1FEDD5483CB33F1EA715D68106180DED2432A293367114A6E325E62F93F73D7ECE4B6A2BCDB829D95C8645C3073B94BA7CB7515CD29042F0967201C6E24A77821E92A6C756DF79841ACBAAE11D90CA03B9FCD24EF9E304B5D35248A7BD70557399960277058AE3E99C7C7E2284858B7BF8B08CDD286964186A50A7FCBCC6A24F00FEE5B9698BBD3B1AEAD0CE81FEA461C0ABD716843A50100040101000500100006E377F547D0D20A4A8ACAE0501098BDE40200070100080008417BD66E6603D401080009417BD66E6603D401".HexToBinary();
            var key = new KeyCredential(blob, DummyDN);
            Assert.AreEqual(KeyCredentialVersion.Version2, key.Version);
            Assert.AreEqual(KeyUsage.NGC, key.Usage);
            Assert.AreEqual(KeySource.ActiveDirectory, key.Source);
            Assert.AreEqual("IHF64FL8z1RqrQ1R6Hiq1pzgT9w59ajY4866a8tNoOc=", key.Identifier);
            Assert.AreEqual("47f577e3-d2d0-4a0a-8aca-e0501098bde4", key.DeviceId.ToString());
            Assert.IsNotNull(key.CustomKeyInfo);

            // Serialize
            byte[] serialized = key.ToByteArray();
            Assert.AreEqual(blob.Length, serialized.Length);
            CollectionAssert.AreEqual(blob, serialized);
        }

        [TestMethod]
        public void KeyCredential_Parse_UserKey2()
        {
            byte[] blob = "000200002000013845C226E299D67EFB43D7504DA462E2D951B517124E5D41A9B0C61E5A15B978200002A2E324776A66AED61D60C771DE3B1B8AA38CF260B63083DB1DA554F233FBF92B1B0103525341310008000003000000000100000000000000000000010001BF723DF58198223D30D10EF3335B1360453A89C57D4B8F0CCE3F958F834F50A01A069E3D92AE0DE07C92A43DF405AC756FFE2C97801E879CED5B0E25E052CEBF352C605C36BF87A2CFC16F830ABCB5A14DDC3EE282313ABE7049C55F2D37164BD050A20C8E5F6CD4B9EDDEC523836EA8DDF0E94ECE5B87A4B6541811312FED6BA0A118E174CCA19352C1A0DB704B9E789C086FB58543554746F4DFCDDD8E5DFEA2A548788DC340FD806A6D6ED6F2003B9E1447AF6A4040FBB2802D9093C3EB432BB72B8F033887555F60E70B927CB6C1FEC2BF17C03FCA03B3BAA56FB4F2A1ECCCD33B6C6AFCBB29CB65304E5894FDD77FD3982D1FB2B2AEAC6B5451F14A1A8F01000401010005011000064B5E981009083D448679149422D9E1700600070100000100000800080040230E430000400800097A9DBAB3B32AD548".HexToBinary();
            var key = new KeyCredential(blob, DummyDN);
            Assert.AreEqual(KeyCredentialVersion.Version2, key.Version);
            Assert.AreEqual(KeyUsage.NGC, key.Usage);
            Assert.AreEqual(KeySource.AzureActiveDirectory, key.Source);
            Assert.IsTrue(key.CustomKeyInfo.SupportsNotification.Value);

            // Serialize
            byte[] serialized = key.ToByteArray();
            Assert.AreEqual(blob.Length, serialized.Length);
            CollectionAssert.AreEqual(blob, serialized);
        }

        [TestMethod]
        public void KeyCredential_Parse_UserKey3()
        {
            byte[] blob = "00020000200001D333C85D306B27498806D9AEFDC984C18DE8EB855BF481E4AD3A24A21929FD5B200002E88E7EBE4CE413FDA9ED987C0CC8C327438B0861DC3BE116680CF8E2BAEB27B01B0103525341310008000003000000000100000000000000000000010001976D21DC9A0C0B84040688F5E7F2BB8147B1305CA01CEFDB13E9FAB49EB6734FD3C32B5D34B01EB6ACE35DDF73E62CB506501A5FD1AAAB698FB98AEA2F2721393C155D84DDF59EF91D8F6402FD755D246C3E04BAF96EFA04BBC7DD314C083800B934B192EA587904C938255D781EC0B2FE8FA3135F952A13FF805492579AD6710051525A7A824A8A5CBA74EF4D3A2F2E271856FF633A411912A53BEAA2805A1B57148ACC8404B473FD3580F450DE5AAB10334FEB084B6045A65840898A66BF88AE19DB802AF7FA4AEED95ECDC8FF286AE0075575F82974396B72730C15C511A961BBD6A5A4B46D395AA85F82ACBD585CE57DAE05EE7B22CBEA9E9E02571EF5890100040101000501100006871059FD5C24F54FA5EAC14DE5E2B32D05000701000000000800080040230E43000040080009D1B7948179CED448".HexToBinary();
            var key = new KeyCredential(blob, DummyDN);
            Assert.AreEqual(KeyCredentialVersion.Version2, key.Version);
            Assert.AreEqual(KeyUsage.NGC, key.Usage);
            Assert.AreEqual(KeySource.AzureActiveDirectory, key.Source);
            Assert.AreEqual(KeyFlags.None, key.CustomKeyInfo.Flags);

            // Serialize
            byte[] serialized = key.ToByteArray();
            Assert.AreEqual(blob.Length, serialized.Length);
            CollectionAssert.AreEqual(blob, serialized);
        }

        [TestMethod]
        public void KeyCredential_Parse_UserKey4()
        {
            byte[] blob = "00020000200001B172047DE6F5926155BEEF8A10E7F16453C371696E61927257E7DE1876ADBE1920000248054C70147BF28076C655A54D7493FFD723DE422E6DF8BFA89C059D8A3E84521B0103525341310008000003000000000100000000000000000000010001B11FDB9CC39BB2C09AA244E8794BF60C7C9A7348DE93E9F368D4FFA77E6B96BB81898D53DA004BA74EC5E5EAE8C67D6DBC126863A78C436357A6C0AF5AF0557E8B1C71319D98CF6ECABED321E0751EAC0FCD2808A02152B0D703AFE0B54C10132CE981E4088A28110F5A4743B5D5A7862A5EAED28F53F2413CE763BCD823EC81EB225EB6A9A9989006E36A574D3FFDBF62BE4BDC00F7014D2E59BFD4077285BE88232BABFC3AEF85E20D8E97C2A94F64902CC86ADC3A2C486CC7CA8D0B163DEB41F1F66D202382D1C5F7DAC30BF9CA6F26538E5E91F3E1CBD8818B58459676588913BABD84E1AE0C2CCF5A76326F81B063581468B55B3E015DDA17B30A436CA10100040101000501100006739E89E927DBF94AB7EBC4201D6577EB040007010000000800080040230E430000400800094C79579CD17CD448".HexToBinary();
            var key = new KeyCredential(blob, DummyDN);
            Assert.AreEqual(KeyCredentialVersion.Version2, key.Version);
            Assert.AreEqual(KeyUsage.NGC, key.Usage);
            Assert.AreEqual(KeySource.AzureActiveDirectory, key.Source);
            Assert.AreEqual(KeyFlags.None, key.CustomKeyInfo.Flags);

            // Serialize
            byte[] serialized = key.ToByteArray();
            Assert.AreEqual(blob.Length, serialized.Length);
            CollectionAssert.AreEqual(blob, serialized);
        }

        [TestMethod]
        public void KeyCredential_Parse_UserKeyFIDO()
        {
            byte[] blob = "000200001000015847BA3C54FEDCC4FEA49D957D1FF88D20000261D774442D358413F23535501330F1FCB80FAA161C090932FA706966C812B724C404037B2276657273696F6E223A312C226175746844617461223A224E577965314B435449626C70587836766B59494438625666614A326D48377957474577566664706F44494846414141414D766F726D6479654F554A586A354A4B4D4E49385152674145466848756A78552F747A452F7153646C5830662B49326C415149444A69414249566767684858674A303148324B3568387A473075642F762B4E6757724F504C726F6B3976364E436D3168666F766B695743435376764C507A456F667878324D67442F4F54337A676C5850587A6357464B36554C575863505A54305862364672614731685979317A5A574E795A585431222C22783563223A5B224D4949437644434341615367417749424167494541363377456A414E42676B71686B69473977304241517346414441754D5377774B6759445651514445794E5A64574A705932386756544A4749464A7662335167513045675532567961574673494451314E7A49774D44597A4D544167467730784E4441344D4445774D4441774D444261474138794D4455774D446B774E4441774D4441774D466F776254454C4D416B474131554542684D4355305578456A415142674E5642416F4D43566C31596D6C6A62794242516A45694D434147413155454377775A515856306147567564476C6A59585276636942426448526C6333526864476C76626A456D4D43514741315545417777645758566961574E764946557952694246525342545A584A70595777674E6A45334D7A41344D7A51775754415442676371686B6A4F5051494242676771686B6A4F50514D4242774E434141515A6E6F65634669323333446E75536B4B675268616C73776E2B79676B766472344A53506C746270584B354D786C7A56536757632B3978386D7A477973646242684565634C41596651597170564C57576F7348506F586F327777616A416942676B724267454541594C4543674945465445754D7934324C6A45754E4334784C6A51784E4467794C6A45754E7A4154426773724267454541594C6C4841494241515145417749454D444168426773724267454541594C6C4841454242415153424244364B356E636E6A6C4356342B53536A4453504545594D41774741315564457745422F7751434D4141774451594A4B6F5A496876634E4151454C425141446767454241436A727332662B30646A77346F6E7279702F32324164587867366135587978636F796248446A4B7537324532534E3971444773495A536644793338444446722F6246317332356A6F69753757413674796C4B4130486D45446C6F654A584A69576A76376832417A322F736971576E4A4F4C6963345845316C4143684A53325841716B536B39564647656C6733534C4F696966724265742B6562645177414C2B325146726352374A7258525147396B557937364F3256635367626450524F7348664F59657977617268616C7956535A2B364F4F594B2F512F444C49614F43306A58726E6B7A6D32796D4D5146516C4241497973725965454D3177786946627744742B6C416362634F4574484566355A6C576937356E557A6C576E386253782F35464F3454625A35684945635569475270694942454D525A6C4F496D345A49625A79636E2F764A4F465254567073305630533479677444633D225D2C22646973706C61794E616D65223A22597562696B65792035227D0100040701000501100006000000000000000000000000000000000F00070101000000000000000000000000000800080040230E430000400800099310993462F6D648".HexToBinary();
            var key = new KeyCredential(blob, DummyDN);
            Assert.AreEqual(KeyCredentialVersion.Version2, key.Version);
            Assert.AreEqual(KeyUsage.FIDO, key.Usage);
            Assert.AreEqual(KeySource.AzureActiveDirectory, key.Source);
            Assert.AreEqual(KeyFlags.Attestation, key.CustomKeyInfo.Flags);
            Assert.AreEqual("WEe6PFT+3MT+pJ2VfR/4jQ==", key.Identifier);

            // Serialize
            byte[] serialized = key.ToByteArray();
            Assert.AreEqual(blob.Length, serialized.Length);
            CollectionAssert.AreEqual(blob, serialized);
        }

        [TestMethod]
        public void KeyCredential_Parse_DeviceKey()
        {
            byte[] blob = "0002000020000173E6BEB8A9B5B0828388476E7BFDD5F8E7A113EC0807EF25C0FBCF39CEB4311120000299DA9872C6EB63882C1200B3B2BECCF3C582418F9FC56905963ADA62E52DF3B31B0103525341310008000003000000000100000000000000000000010001B40D7085917A30D2F0D434FEF57477099FFFEBC79F28EB414BB75C86B4B5CAC0D9E6ACA86EB8126EDB724AF40FD773A7F14732A7ED862A0828A367194FB3D61EC6EA15CB450597F3BAA64E4974B255D0819E06B58B47C858C384B88E27D0EA52F962A592B115EEA3AA21A6A5185DD58F5D779118717FD07C8CAF50F5F078BFC3AED355BB2F78E8C48C4F6DA2BD679CDCD1C0ED8320F5BC9EC6545E4E7CD9AA7642E180E2A3AD20BCCCF3C30A34BEDF27835528BE955A7599D42869339218936E78FF6D46BEEE0097F2DECB2791F7842BB55BA639A44F659F547B5AA1E959370ACBC908248D05893D539F7E4E6BE834CCF0A3101879717585D015992B3C9407410100040201000500100006E377F547D0D20A4A8ACAE0501098BDE40200070100080008405E47D3C301D401080009405E47D3C301D401".HexToBinary();
            var key = new KeyCredential(blob, DummyDN);
            Assert.AreEqual(KeyCredentialVersion.Version2, key.Version);
            Assert.AreEqual(KeyUsage.STK, key.Usage);
            Assert.AreEqual(KeySource.ActiveDirectory, key.Source);
            Assert.AreEqual("c+a+uKm1sIKDiEdue/3V+OehE+wIB+8lwPvPOc60MRE=", key.Identifier);
            Assert.IsNotNull(key.CustomKeyInfo);

            // Serialize
            byte[] serialized = key.ToByteArray();
            Assert.AreEqual(blob.Length, serialized.Length);
            CollectionAssert.AreEqual(blob, serialized);
        }

        [TestMethod]
        public void KeyCredential_Parse_ComputerKey()
        {
            byte[] blob = "000200002000019C00E026B615793DE47951FF58A15F1F967297980C3EDAAF60B9E08FC9986F1320000204B8D485B4691C934E291D38B873B78390D4074B5D5391A851BF12C7466FEEC40E01033082010A0282010100B851C9219527F52E8A51582243E2CCA390B634FE5DE16B2BCA2E225257F3FF20BFE478C98B36095C49D897D42A67E2545D77003D38B9DF18682AF6FBFF281895CE61DADD5F72E13B40DA34E47833D380E58175F7D509DFA5E9971068756626AF1425B7CE0393BDB28AFF8E25CC601DE4542672E723B5BBB4E7D3963C2ACFB445171B43C14683DF0ED6524BD11F583D5BBEEBBA1DE6DE3384DF598E0D8BADACFBF1667890DC72CE61AF746084364BC288D982F23A6CD123E9BB6B701E00B096BE899876FE93BDD8B1C56FC107F36F7B2C8CE1AFB715FCDECA192634BE961B6104F21BFD84C97305123FF69D05D685CC8760CE54D9788457882D9DD39AFDA1D77D0203010001010004010100050008000831FF708C3402D401080009A962ABD55AF6D301".HexToBinary();

            // Parse
            var key = new KeyCredential(blob, DummyDN);
            Assert.AreEqual(KeyCredentialVersion.Version2, key.Version);
            Assert.AreEqual(KeyUsage.NGC, key.Usage);
            Assert.AreEqual(KeySource.ActiveDirectory, key.Source);
            Assert.IsNull(key.CustomKeyInfo);

            // Serialize
            byte[] serialized = key.ToByteArray();
            Assert.AreEqual(blob.Length, serialized.Length);
            CollectionAssert.AreEqual(blob, serialized);
        }

        [TestMethod]
        public void KeyCredential_Generate_UserPublicKey()
        {
            // Input
            byte[] inputBinaryCertificate = "308205473082042fa00302010202104dc51a1f078cd9834fb0523834a5402e300d06092a864886f70d010105050030820149318201453082014106035504031e8201380053002d0031002d0035002d00320031002d0032003800390034003400380039003800340037002d0031003100330036003400320038003400340037002d0032003100340033003000390038003800370034002d0031003100300036002f00380032003200370063006600650061002d0061006600350033002d0034006200370035002d0061003400650033002d006500660035003800320061003500320037003400310065002f006c006f00670069006e002e00770069006e0064006f00770073002e006e00650074002f00330038003300610033003800380039002d0035006200630039002d0034003700610033002d0038003400360063002d003200620037003000660030006200370066006500300065002f00610064006d0069006e00400063006f006e0074006f0073006f002e0063006f006d301e170d3138303631333232323233345a170d3438303631333232333233345a30820149318201453082014106035504031e8201380053002d0031002d0035002d00320031002d0032003800390034003400380039003800340037002d0031003100330036003400320038003400340037002d0032003100340033003000390038003800370034002d0031003100300036002f00380032003200370063006600650061002d0061006600350033002d0034006200370035002d0061003400650033002d006500660035003800320061003500320037003400310065002f006c006f00670069006e002e00770069006e0064006f00770073002e006e00650074002f00330038003300610033003800380039002d0035006200630039002d0034003700610033002d0038003400360063002d003200620037003000660030006200370066006500300065002f00610064006d0069006e00400063006f006e0074006f0073006f002e0063006f006d30820122300d06092a864886f70d01010105000382010f003082010a0282010100c1a78914457758b0b13c70c710c7f8548f3f9ed56ad4640b6e6a112655c98ecac1cbd68a298f5686c08439428a97fe6fdf58d78ea481905182bad684c2d9c5cde1cde34aa19742e8bbf58b953eac4c562fcf598cc176b02dbe9fffef5937a65815c236f92892f7e511a1fedd5483cb33f1ea715d68106180ded2432a293367114a6e325e62f93f73d7ece4b6a2bcdb829d95c8645c3073b94ba7cb7515cd29042f0967201c6e24a77821e92a6c756df79841acbaae11d90ca03b9fcd24ef9e304b5d35248a7bd70557399960277058ae3e99c7c7e2284858b7bf8b08cdd286964186a50a7fcbcc6a24f00fee5b9698bbd3b1aead0ce81fea461c0abd716843a50203010001a3273025300c0603551d130101ff0402300030150603551d25040e300c060a2b060104018237140202300d06092a864886f70d01010505000382010100435e76ffbe9b75052f1b96f67439d3970821f17be454703cb36be91d4c4de349a7d841b266412fa4235254d774f0a224708a4af78e6dce3fd42a2f89365323c951762a38e8b3d2ba0dd0971bf1cb0ecaa17cd82cdb64b969df48419aa4f28cc2d8c91112274ae7ba2f4a0db3c55b7b34ad0a9a5dc56f195208cb7440e9a51bd6996422a31c1632be6335300460538b80e7282f1bb5331cdbda2a182fe33bef9980e6a5257d264e13749036d508da8c29cde953cb747330a99483111abe69a49d11f426f30514505afe25e28a0ec5011e293bf3e13295f7d89a532b35a9e6bb7166ee21247a8dd2000ee987464748b838aa689cc6a499ea2e6614bb100c3beea4".HexToBinary();
            string expectedPublicKeyBlob = "525341310008000003000000000100000000000000000000010001C1A78914457758B0B13C70C710C7F8548F3F9ED56AD4640B6E6A112655C98ECAC1CBD68A298F5686C08439428A97FE6FDF58D78EA481905182BAD684C2D9C5CDE1CDE34AA19742E8BBF58B953EAC4C562FCF598CC176B02DBE9FFFEF5937A65815C236F92892F7E511A1FEDD5483CB33F1EA715D68106180DED2432A293367114A6E325E62F93F73D7ECE4B6A2BCDB829D95C8645C3073B94BA7CB7515CD29042F0967201C6E24A77821E92A6C756DF79841ACBAAE11D90CA03B9FCD24EF9E304B5D35248A7BD70557399960277058AE3E99C7C7E2284858B7BF8B08CDD286964186A50A7FCBCC6A24F00FEE5B9698BBD3B1AEAD0CE81FEA461C0ABD716843A5";
            var certificate = new X509Certificate2(inputBinaryCertificate);
            var expectedRSAParameters = ((RSACryptoServiceProvider)certificate.PublicKey.Key).ExportParameters(false);
            string expectedModulus = Convert.ToBase64String(expectedRSAParameters.Modulus);

            // Convert
            byte[] publicKeyBlob = certificate.ExportPublicKeyBlob();
            var key = new KeyCredential(certificate, Guid.NewGuid(), DummyDN);

            // Check
            Assert.AreEqual(expectedPublicKeyBlob, publicKeyBlob.ToHex(true));
            CollectionAssert.AreEqual(expectedRSAParameters.Modulus, key.NGCPublicKey.Value.Modulus);
            CollectionAssert.AreEqual(expectedRSAParameters.Exponent, key.NGCPublicKey.Value.Exponent);
            Assert.AreEqual(expectedModulus, key.NGCModulus);
        }

        [TestMethod]
        public void KeyCredential_Generate_UserKey()
        {
            byte[] publicKey = "525341310008000003000000000100000000000000000000010001C1A78914457758B0B13C70C710C7F8548F3F9ED56AD4640B6E6A112655C98ECAC1CBD68A298F5686C08439428A97FE6FDF58D78EA481905182BAD684C2D9C5CDE1CDE34AA19742E8BBF58B953EAC4C562FCF598CC176B02DBE9FFFEF5937A65815C236F92892F7E511A1FEDD5483CB33F1EA715D68106180DED2432A293367114A6E325E62F93F73D7ECE4B6A2BCDB829D95C8645C3073B94BA7CB7515CD29042F0967201C6E24A77821E92A6C756DF79841ACBAAE11D90CA03B9FCD24EF9E304B5D35248A7BD70557399960277058AE3E99C7C7E2284858B7BF8B08CDD286964186A50A7FCBCC6A24F00FEE5B9698BBD3B1AEAD0CE81FEA461C0ABD716843A5".HexToBinary();
            Guid deviceId = Guid.Parse("47f577e3-d2d0-4a0a-8aca-e0501098bde4");
            DateTime creationTime = DateTime.FromFileTime(131734027581684545);
            string expectedKeyCredentialBlob = "0002000020000120717AE052FCCF546AAD0D51E878AAD69CE04FDC39F5A8D8E3CEBA6BCB4DA0E720000214B7474E61C1001D3E546CFED8E387CFC1AC86A2CA7B3CDCF1267614585E2A341B0103525341310008000003000000000100000000000000000000010001C1A78914457758B0B13C70C710C7F8548F3F9ED56AD4640B6E6A112655C98ECAC1CBD68A298F5686C08439428A97FE6FDF58D78EA481905182BAD684C2D9C5CDE1CDE34AA19742E8BBF58B953EAC4C562FCF598CC176B02DBE9FFFEF5937A65815C236F92892F7E511A1FEDD5483CB33F1EA715D68106180DED2432A293367114A6E325E62F93F73D7ECE4B6A2BCDB829D95C8645C3073B94BA7CB7515CD29042F0967201C6E24A77821E92A6C756DF79841ACBAAE11D90CA03B9FCD24EF9E304B5D35248A7BD70557399960277058AE3E99C7C7E2284858B7BF8B08CDD286964186A50A7FCBCC6A24F00FEE5B9698BBD3B1AEAD0CE81FEA461C0ABD716843A50100040101000500100006E377F547D0D20A4A8ACAE0501098BDE40200070100080008417BD66E6603D401080009417BD66E6603D401";

            var keyCredential = new KeyCredential(publicKey, deviceId, DummyDN, creationTime);

            byte[] keyCredentialBlob = keyCredential.ToByteArray();
            Assert.AreEqual(expectedKeyCredentialBlob, keyCredentialBlob.ToHex(true));
        }
    }
}
