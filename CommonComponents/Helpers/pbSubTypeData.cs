using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace PasswordBoss
{
	// incorrect analytics by using linked file
	[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal static class PBSubType
	{
		[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
		internal static class PasswordVault
		{
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class Login
			{
				public string SiteName { get; set; }
				public string Url { get; set; }
				public string UserName { get; set; }
				public string Notes { get; set; }
				public string Password { get; set; }
				public bool ReenterPassword { get; set; }
				public bool Autologin { get; set; }
				public bool Subdomain { get; set; }
				public bool UseSecureBrowser { get; set; }
				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public Login()
				{
					this.SiteName = "";
					this.Url = "";
					this.UserName = "";
					this.Notes = "";
					this.Password = "";
					this.ReenterPassword = false;
					this.Autologin = false;
					this.Subdomain = false;
					this.UseSecureBrowser = false;

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryOther;
					SelectedCategory.Name = DefaultCategories.CategoryOther;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}



				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_PasswordVault,
						Type = DefaultProperties.SecurityItemSubType_PV_Login,
						Site = new Site()
						{
							Name = this.SiteName,
							Uri = this.Url
						},
						Name = this.SiteName,
						Data = new SecureItemData()
						{
							username = this.UserName,//!_common.IsEmailValid(this.UserName) ? this.UserName : null,
							//email = _common.IsEmailValid(this.UserName) ? this.UserName : null,
							// nickname = SiteName,
							re_enter_password = this.ReenterPassword, //ReenterMasterPassword,
							autologin = this.Autologin, //AutoLogin,
							notes = this.Notes,
							password = this.Password,
							sub_domain = this.Subdomain, //ThisSubdomainOnly,
							use_secure_browser = this.UseSecureBrowser, //UseSecureBrowser
						},
						Folder = this.SelectedCategory
					};

					return secureItem;
				}
			}
		}

		[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
		internal static class DigitalWallet
		{
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class PayPal
			{
				public string PaypalNickname { get; set; }
				public string Username { get; set; }
				public string Password { get; set; }
				public string PaypalNotes { get; set; }
				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public PayPal()
				{
					PaypalNickname = "";
					Username = "";
					Password = "";
					PaypalNotes = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryPaypal;
					SelectedCategory.Name = DefaultCategories.CategoryPaypal;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}



				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_DigitalWallet,
						Name = PaypalNickname,
						Type = DefaultProperties.SecurityItemSubType_DW_Paypal,
						Data = new SecureItemData()
						{
							username = Username,
							password = Password,
							notes = PaypalNotes
						},
						Folder = SelectedCategory,
					};

					return secureItem;
				}


			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class Bank
			{
				public string AccountNickname { get; set; }
				public string AccountBankName { get; set; }
				public string AccountName { get; set; }
				public string AccountRoutingNumber { get; set; }
				public string AccountNumber { get; set; }
				public string Swift { get; set; }
				public string Iban { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public Bank()
				{
					AccountNickname = "";
					AccountBankName = "";
					AccountName = "";
					AccountRoutingNumber = "";
					AccountNumber = "";
					Swift = "";
					Iban = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryBankAccount;
					SelectedCategory.Name = DefaultCategories.CategoryBankAccount;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}

				public SecureItem GetSecureItem()
				{
					SecureItem si = new SecureItem();
					si.SecureItemTypeName = DefaultProperties.SecurityItemType_DigitalWallet;
					si.Type = DefaultProperties.SecurityItemSubType_DW_Bank;
					si.Name = AccountNickname;
					si.Data = new SecureItemData();
					si.Data.bank_name = AccountBankName;
					si.Data.nameOnAccount = AccountName;
					si.Data.routingNumber = AccountRoutingNumber;
					si.Data.accountNumber = AccountNumber;
					si.Data.swift = Swift;
					si.Data.iban = Iban;

					return si;
				}

			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class CreditCard
			{
				public string CreditCardNickname { get; set; }
				public KeyValuePair<string, string>? SelectedCountry { get; set; }
				public string NameOnCard { get; set; }
				public string CardNumber { get; set; }
				public KeyValuePair<string, string>? SelectedCreditCard { get; set; }
				public string CreditCardType { get; set; }
				public string ExpiresMonth { get; set; }
				public string ExpiresYear { get; set; }
				public string IssueBank { get; set; }
				public string Cvv { get; set; }
				public string Pin { get; set; }
				public SecureItem SelectedAddress { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public CreditCard()
				{
					CreditCardNickname = "";
					this.SelectedCountry = null;
					NameOnCard = "";
					CardNumber = "";
					SelectedCreditCard = null;
					CreditCardType = "";
					ExpiresMonth = "";
					ExpiresYear = "";
					IssueBank = "";
					Cvv = "";
					Pin = "";
					SelectedAddress = new SecureItem();

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryCreditCard;
					SelectedCategory.Name = DefaultCategories.CategoryCreditCard;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}

				List<KeyValuePair<string, string>> creditCardList = new List<KeyValuePair<string, string>>(){
						new KeyValuePair<string, string>("Master", "Master"),
						new KeyValuePair<string, string>("Visa", "Visa"),
						new KeyValuePair<string, string>("American Express", "American Express"),
						new KeyValuePair<string, string>("JCB", "JCB"),
						new KeyValuePair<string, string>("Discover", "Discover"),
						new KeyValuePair<string, string>("Other", "Other")
					};

				public SecureItem GetSecureItem()
				{
					if (!String.IsNullOrEmpty(CreditCardType))
					{
						SelectedCreditCard = creditCardList.FirstOrDefault(x => x.Key.ToUpperInvariant().Contains(CreditCardType.ToUpperInvariant()));
						if (SelectedCreditCard.Value.Key == null)
							SelectedCreditCard = creditCardList.FirstOrDefault(x => x.Value.ToUpperInvariant().Contains(CreditCardType.ToUpperInvariant()));
						if (SelectedCreditCard.Value.Key == null)
							SelectedCreditCard = creditCardList.FirstOrDefault(x => CreditCardType.ToUpperInvariant().Contains(x.Key.ToUpperInvariant()));
						if (SelectedCreditCard.Value.Key == null)
							SelectedCreditCard = creditCardList.FirstOrDefault(x => CreditCardType.ToUpperInvariant().Contains(x.Value.ToUpperInvariant()));

						if (SelectedCreditCard.Value.Key == null)
							SelectedCreditCard = creditCardList.FirstOrDefault(x => x.Key == "Other");
					}
					else
					{
						SelectedCreditCard = creditCardList.FirstOrDefault(x => x.Key == "Other");
					}

					Func<string, string, string> createExpires = (year, month) =>
					{
						if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
						{
							return null;
						}
						else
						{
							var yearParsed = int.Parse(ExpiresYear, CultureInfo.InvariantCulture);
							var monthParsed = int.Parse(ExpiresMonth, CultureInfo.InvariantCulture);
							var daysInMonth = DateTime.DaysInMonth(yearParsed, monthParsed);
							var result = new DateTime(yearParsed, monthParsed, daysInMonth);

							result.AddHours(23).AddMinutes(59).AddSeconds(59);
							return result.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
						}
					};

					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_DigitalWallet,
						Name = CreditCardNickname,
						Type = DefaultProperties.SecurityItemSubType_DW_CreditCard,
						Color = "0",
						Data = new SecureItemData()
						{
							country = SelectedCountry.HasValue ? SelectedCountry.Value.Key : null,
							nameOnCard = NameOnCard,
							cardNumber = CardNumber,
							cardType = SelectedCreditCard.HasValue ? SelectedCreditCard.Value.Key : null,
							expires = createExpires(ExpiresYear, ExpiresMonth),

							issuingBank = IssueBank,
							security_code = Cvv,
							pin = Pin,
							addressRef = SelectedAddress != null ? SelectedAddress.Id : null
						},
						Folder = SelectedCategory,
					};

					return secureItem;
				}
			}

		}

		[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
		internal static class PersonalInfo
		{
			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class Name
			{
				public string FirstName { get; set; }
				public string LastName { get; set; }
				public string MiddleName { get; set; }
				public string Notes { get; set; }
				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public Name()
				{
					FirstName = "";
					LastName = "";
					MiddleName = "";
					Notes = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryNames;
					SelectedCategory.Name = DefaultCategories.CategoryNames;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}

				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
						Name = String.Format(CultureInfo.InvariantCulture, "{0} {1}", FirstName, LastName),
						Type = DefaultProperties.SecurityItemSubType_PI_Names,
						Data = new SecureItemData()
						{
							firstName = this.FirstName,
							middleName = this.MiddleName,
							lastName = this.LastName,
							notes = this.Notes
						},
						Folder = this.SelectedCategory
					};

					return secureItem;
				}
			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class Address
			{
				public KeyValuePair<string, string>? SelectedCountry { get; set; }
				public string Nickname { get; set; }
				public string Address1 { get; set; }
				public string Address2 { get; set; }
				public string AptSuit { get; set; }
				public string City { get; set; }
				public string State { get; set; }
				public string ZipCode { get; set; }
				public string Notes { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public Address()
				{
					SelectedCountry = new KeyValuePair<string, string>?();
					Nickname = "";
					Address1 = "";
					Address2 = "";
					AptSuit = "";
					City = "";
					State = "";
					ZipCode = "";
					Notes = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryAddresses;
					SelectedCategory.Name = DefaultCategories.CategoryAddresses;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}



				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
						Name = Nickname,
						Type = DefaultProperties.SecurityItemSubType_PI_Address,
						Data = new SecureItemData()
						{
							country = SelectedCountry.HasValue ? SelectedCountry.Value.Key : null,
							address1 = Address1,
							address2 = Address2,
							apt = AptSuit,
							city = City,
							state = State,
							zipCode = ZipCode,
							notes = Notes
						},
						Folder = SelectedCategory
					};

					return secureItem;
				}
			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class PhoneNumber
			{
				public string Nickname { get; set; }
				public string Number { get; set; }
				public string Notes { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public PhoneNumber()
				{
					Nickname = "";
					Number = "";
					Notes = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryPhoneNumbers;
					SelectedCategory.Name = DefaultCategories.CategoryPhoneNumbers;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}



				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
						Name = Nickname,
						Type = DefaultProperties.SecurityItemSubType_PI_PhoneNumber,
						Data = new SecureItemData()
						{
							phoneNumber = Number,
							notes = Notes
						},
						Folder = SelectedCategory
					};

					return secureItem;
				}
			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class Company
			{
				public string Name { get; set; }
				public string Notes { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public Company()
				{
					Name = "";
					Notes = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryCompany;
					SelectedCategory.Name = DefaultCategories.CategoryCompany;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}



				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
						Name = Name,
						Type = DefaultProperties.SecurityItemSubType_PI_Company,
						Data = new SecureItemData()
						{
							notes = Notes
						},
						Folder = SelectedCategory
					};

					return secureItem;
				}
			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class Email
			{
				public string NickName { get; set; }
				public string Address { get; set; }
				public string Notes { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public Email()
				{
					NickName = "";
					Address = "";
					Notes = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryEmail;
					SelectedCategory.Name = DefaultCategories.CategoryEmail;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}


				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
						Name = NickName,
						Type = DefaultProperties.SecurityItemSubType_PI_Email,
						Data = new SecureItemData()
						{
							email = Address,
							notes = Notes
						},
						Folder = SelectedCategory
					};

					return secureItem;
				}
			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class DriverLicense
			{
				public string LicenseNickname { get; set; }
				public string LicenseFirstName { get; set; }
				public string LicenseLastName { get; set; }
				public string LicenseCountry { get; set; }
				public string LicenseState { get; set; }
				public string LicenseNumber { get; set; }
				public string LicenseExpires { get; set; }
				public string LicenseIssueDate { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public DriverLicense()
				{
					LicenseNickname = "";
					LicenseFirstName = "";
					LicenseLastName = "";
					LicenseCountry = "";
					LicenseState = "";
					LicenseNumber = "";
					LicenseExpires = "";
					LicenseIssueDate = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryDriverLicense;
					SelectedCategory.Name = DefaultCategories.CategoryDriverLicense;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}



				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_SecureNotes,
						Name = LicenseNickname,
						Type = DefaultProperties.SecurityItemSubType_SN_DriverLicense,
						Data = new SecureItemData()
						{
							firstName = LicenseFirstName,
							lastName = LicenseLastName,
							country = LicenseCountry,
							state = LicenseState,
							driverLicenceNumber = LicenseNumber,
							expires = LicenseExpires,
							issueDate = LicenseIssueDate
						},
						Folder = SelectedCategory
					};

					return secureItem;
				}
			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class Passport
			{
				public string PassportNickName { get; set; }
				public string PassportFirstName { get; set; }
				public string PassportLastName { get; set; }
				public string PassportNationality { get; set; }
				public string PassportDateOfBirth { get; set; }
				public string PassportNumber { get; set; }
				public string PassportIssueDate { get; set; }
				public string PassportExpiers { get; set; }
				public string PassportPlaceOfIssue { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public Passport()
				{
					PassportNickName = "";
					PassportFirstName = "";
					PassportLastName = "";
					PassportNationality = "";
					PassportDateOfBirth = "";
					PassportNumber = "";
					PassportIssueDate = "";
					PassportExpiers = "";
					PassportPlaceOfIssue = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryPassport;
					SelectedCategory.Name = DefaultCategories.CategoryPassport;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}

				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_SecureNotes,
						Name = PassportNickName,
						Type = DefaultProperties.SecurityItemSubType_SN_Passport,
						Data = new SecureItemData()
						{
							firstName = PassportFirstName,
							lastName = PassportLastName,
							nationality = PassportNationality,
							dateOfBirth = PassportDateOfBirth,
							passportNumber = PassportNumber,
							issueDate = PassportIssueDate,
							expires = PassportExpiers,
							placeOfIssue = PassportPlaceOfIssue
						}
					};

					return secureItem;
				}
			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class SocialSecurity
			{
				public string SocialSecurityFirstName { get; set; }
				public string SocialSecurityLastName { get; set; }
				public string SocialSecurityNationality { get; set; }
				public string SocialSecurityDateOfBirth { get; set; }
				public string SocialSecurityNumber { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public SocialSecurity()
				{
					SocialSecurityFirstName = "";
					SocialSecurityLastName = "";
					SocialSecurityNationality = "";
					SocialSecurityDateOfBirth = "";
					SocialSecurityNumber = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategorySocialSecurity;
					SelectedCategory.Name = DefaultCategories.CategorySocialSecurity;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}



				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
						Name = String.Format(CultureInfo.InvariantCulture, "{0} {1}", SocialSecurityFirstName, SocialSecurityLastName),
						Type = DefaultProperties.SecurityItemSubType_PI_SocialSecurity,
						Data = new SecureItemData()
						{
							nationality = SocialSecurityNationality,
							firstName = SocialSecurityFirstName,
							lastName = SocialSecurityLastName,
							dateOfBirth = SocialSecurityDateOfBirth,
							ssn = SocialSecurityNumber
						}
					};

					return secureItem;
				}
			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class SecureNotes
			{
				public string Title { get; set; }
				public string Notes { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public SecureNotes()
				{
					this.Title = "";
					this.Notes = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategorySecureNotes;
					SelectedCategory.Name = DefaultCategories.CategorySecureNotes;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}

				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
						Name = this.Title,
						Type = DefaultProperties.SecurityItemSubType_PI_SecureNotes,
						Data = new SecureItemData()
						{
							notes = this.Notes
						},
						Folder = this.SelectedCategory
					};

					return secureItem;
				}

			}

			// incorrect analytics by using linked file
			[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
			[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
			internal class MemberIDs
			{
				public string MemberIdNickname { get; set; }
				public string MemberIdFirstName { get; set; }
				public string MemberIdLastName { get; set; }
				public string MemberId { get; set; }
				public string MemberIdNotes { get; set; }

				public PasswordBoss.DTO.Folder SelectedCategory { get; set; }

				public MemberIDs()
				{
					MemberIdNickname = "";
					MemberIdFirstName = "";
					MemberIdLastName = "";
					MemberId = "";
					MemberIdNotes = "";

					SelectedCategory = new Folder();
					SelectedCategory.Id = DefaultCategories.CategoryMemberIDs;
					SelectedCategory.Name = DefaultCategories.CategoryMemberIDs;
					// SelectedCategory.Deletable = false;
					//SelectedCategory.UseSecureBrowser = false;
				}



				public SecureItem GetSecureItem()
				{
					SecureItem secureItem = new SecureItem()
					{
						SecureItemTypeName = DefaultProperties.SecurityItemType_SecureNotes,
						Name = MemberIdNickname,
						Type = DefaultProperties.SecurityItemSubType_SN_MemberIDs,
						Data = new SecureItemData()
						{
							firstName = MemberIdFirstName,
							lastName = MemberIdLastName,
							memberID = MemberId,
							notes = MemberIdNotes
						},
						Folder = SelectedCategory
					};

					return secureItem;
				}
			}
		}
	}
}