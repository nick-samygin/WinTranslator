using PasswordBoss.DTO;
using PasswordBoss.Performance;
using System.Security.Cryptography;
using System.Text;

namespace PasswordBoss.BusinessLayer
{
	internal class GenerateKeysStepResult : StepResultBase
	{
		internal byte[] PublicKey { get; set; }
		internal ProtectedDataBlock PrivateKey { get; set; }

		public override string ToString()
		{
			return string.Format("{0}; PublicKey- {1}; PrivateKey.Lenght - {2}", base.ToString(), PublicKey.ToString(), PrivateKey.DataLenght);
		}
	}
	//internal class GenerateKeysStep : StepAsyncBase<GenerateKeysStepResult>
	internal class GenerateKeysStep : StepBase<GenerateKeysStepResult>
	{
		public GenerateKeysStep() : base(null)
		{

		}

		protected override GenerateKeysStepResult ExecuteInternal()
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
			{
				var publicKey = Encoding.UTF8.GetBytes(RSAKeyManagement.ExportPublicKeyToPEM(rsa));
				var privateKey = new ProtectedDataBlock(Encoding.UTF8.GetBytes(RSAKeyManagement.ExportPrivateKeyToPEM(rsa)));

				return new GenerateKeysStepResult()
				{
					PrivateKey = privateKey,
					PublicKey = publicKey
				};
			}
		}
	}
}
