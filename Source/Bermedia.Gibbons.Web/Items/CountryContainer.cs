using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Country Container", Description = "Container for countries")]
	[RestrictParents(typeof(RootItem))]
	public class CountryContainer : BaseContentItem, ISelfPopulator
	{
		public CountryContainer()
		{
			this.Name = this.Title = "Countries";
		}

		public override string TemplateUrl
		{
			get { return "~/Admin/View.aspx?selected=" + this.Path; }
		}

		protected override string IconName
		{
			get { return "world"; }
		}

		public void Populate()
		{
			this.Children.Add(new Country("AF", "AFGHANISTAN", "Afghanistan", "AFG", "004"));
			this.Children.Add(new Country("AL", "ALBANIA", "Albania", "ALB", "008"));
			this.Children.Add(new Country("DZ", "ALGERIA", "Algeria", "DZA", "012"));
			this.Children.Add(new Country("AS", "AMERICAN SAMOA", "American Samoa", "ASM", "016"));
			this.Children.Add(new Country("AD", "ANDORRA", "Andorra", "AND", "020"));
			this.Children.Add(new Country("AO", "ANGOLA", "Angola", "AGO", "024"));
			this.Children.Add(new Country("AI", "ANGUILLA", "Anguilla", "AIA", "660"));
			this.Children.Add(new Country("AQ", "ANTARCTICA", "Antarctica", "ATA", "010"));
			this.Children.Add(new Country("AG", "ANTIGUA AND BARBUDA", "Antigua and Barbuda", "ATG", "028"));
			this.Children.Add(new Country("AR", "ARGENTINA", "Argentina", "ARG", "032"));
			this.Children.Add(new Country("AM", "ARMENIA", "Armenia", "ARM", "051"));
			this.Children.Add(new Country("AW", "ARUBA", "Aruba", "ABW", "533"));
			this.Children.Add(new Country("AU", "AUSTRALIA", "Australia", "AUS", "036"));
			this.Children.Add(new Country("AT", "AUSTRIA", "Austria", "AUT", "040"));
			this.Children.Add(new Country("AZ", "AZERBAIJAN", "Azerbaijan", "AZE", "031"));
			this.Children.Add(new Country("BS", "BAHAMAS", "Bahamas", "BHS", "044"));
			this.Children.Add(new Country("BH", "BAHRAIN", "Bahrain", "BHR", "048"));
			this.Children.Add(new Country("BD", "BANGLADESH", "Bangladesh", "BGD", "050"));
			this.Children.Add(new Country("BB", "BARBADOS", "Barbados", "BRB", "052"));
			this.Children.Add(new Country("BY", "BELARUS", "Belarus", "BLR", "112"));
			this.Children.Add(new Country("BE", "BELGIUM", "Belgium", "BEL", "056"));
			this.Children.Add(new Country("BZ", "BELIZE", "Belize", "BLZ", "084"));
			this.Children.Add(new Country("BJ", "BENIN", "Benin", "BEN", "204"));
			this.Children.Add(new Country("BM", "BERMUDA", "Bermuda", "BMU", "060"));
			this.Children.Add(new Country("BT", "BHUTAN", "Bhutan", "BTN", "064"));
			this.Children.Add(new Country("BO", "BOLIVIA", "Bolivia", "BOL", "068"));
			this.Children.Add(new Country("BA", "BOSNIA AND HERZEGOVINA", "Bosnia and Herzegovina", "BIH", "070"));
			this.Children.Add(new Country("BW", "BOTSWANA", "Botswana", "BWA", "072"));
			this.Children.Add(new Country("BV", "BOUVET ISLAND", "Bouvet Island", "BVT", "074"));
			this.Children.Add(new Country("BR", "BRAZIL", "Brazil", "BRA", "076"));
			this.Children.Add(new Country("IO", "BRITISH INDIAN OCEAN TERRITORY", "British Indian Ocean Territory", "IOT", "086"));
			this.Children.Add(new Country("BN", "BRUNEI DARUSSALAM", "Brunei Darussalam", "BRN", "096"));
			this.Children.Add(new Country("BG", "BULGARIA", "Bulgaria", "BGR", "100"));
			this.Children.Add(new Country("BF", "BURKINA FASO", "Burkina Faso", "BFA", "854"));
			this.Children.Add(new Country("BI", "BURUNDI", "Burundi", "BDI", "108"));
			this.Children.Add(new Country("KH", "CAMBODIA", "Cambodia", "KHM", "116"));
			this.Children.Add(new Country("CM", "CAMEROON", "Cameroon", "CMR", "120"));
			this.Children.Add(new Country("CA", "CANADA", "Canada", "CAN", "124"));
			this.Children.Add(new Country("CV", "CAPE VERDE", "Cape Verde", "CPV", "132"));
			this.Children.Add(new Country("KY", "CAYMAN ISLANDS", "Cayman Islands", "CYM", "136"));
			this.Children.Add(new Country("CF", "CENTRAL AFRICAN REPUBLIC", "Central African Republic", "CAF", "140"));
			this.Children.Add(new Country("TD", "CHAD", "Chad", "TCD", "148"));
			this.Children.Add(new Country("CL", "CHILE", "Chile", "CHL", "152"));
			this.Children.Add(new Country("CN", "CHINA", "China", "CHN", "156"));
			this.Children.Add(new Country("CX", "CHRISTMAS ISLAND", "Christmas Island", "CXR", "162"));
			this.Children.Add(new Country("CC", "COCOS (KEELING)) ISLANDS", "Cocos (Keeling)) Islands", "CCK", "166"));
			this.Children.Add(new Country("CO", "COLOMBIA", "Colombia", "COL", "170"));
			this.Children.Add(new Country("KM", "COMOROS", "Comoros", "COM", "174"));
			this.Children.Add(new Country("CG", "CONGO", "Congo", "COG", "178"));
			this.Children.Add(new Country("CD", "CONGO, THE DEMOCRATIC REPUBLIC OF THE", "Congo, the Democratic Republic of the", "COD", "180"));
			this.Children.Add(new Country("CK", "COOK ISLANDS", "Cook Islands", "COK", "184"));
			this.Children.Add(new Country("CR", "COSTA RICA", "Costa Rica", "CRI", "188"));
			this.Children.Add(new Country("CI", "COTE D'IVOIRE", "Cote D'Ivoire", "CIV", "384"));
			this.Children.Add(new Country("HR", "CROATIA", "Croatia", "HRV", "191"));
			this.Children.Add(new Country("CU", "CUBA", "Cuba", "CUB", "192"));
			this.Children.Add(new Country("CY", "CYPRUS", "Cyprus", "CYP", "196"));
			this.Children.Add(new Country("CZ", "CZECH REPUBLIC", "Czech Republic", "CZE", "203"));
			this.Children.Add(new Country("DK", "DENMARK", "Denmark", "DNK", "208"));
			this.Children.Add(new Country("DJ", "DJIBOUTI", "Djibouti", "DJI", "262"));
			this.Children.Add(new Country("DM", "DOMINICA", "Dominica", "DMA", "212"));
			this.Children.Add(new Country("DO", "DOMINICAN REPUBLIC", "Dominican Republic", "DOM", "214"));
			this.Children.Add(new Country("EC", "ECUADOR", "Ecuador", "ECU", "218"));
			this.Children.Add(new Country("EG", "EGYPT", "Egypt", "EGY", "818"));
			this.Children.Add(new Country("SV", "EL SALVADOR", "El Salvador", "SLV", "222"));
			this.Children.Add(new Country("GQ", "EQUATORIAL GUINEA", "Equatorial Guinea", "GNQ", "226"));
			this.Children.Add(new Country("ER", "ERITREA", "Eritrea", "ERI", "232"));
			this.Children.Add(new Country("EE", "ESTONIA", "Estonia", "EST", "233"));
			this.Children.Add(new Country("ET", "ETHIOPIA", "Ethiopia", "ETH", "231"));
			this.Children.Add(new Country("FK", "FALKLAND ISLANDS (MALVINAS))", "Falkland Islands (Malvinas))", "FLK", "238"));
			this.Children.Add(new Country("FO", "FAROE ISLANDS", "Faroe Islands", "FRO", "234"));
			this.Children.Add(new Country("FJ", "FIJI", "Fiji", "FJI", "242"));
			this.Children.Add(new Country("FI", "FINLAND", "Finland", "FIN", "246"));
			this.Children.Add(new Country("FR", "FRANCE", "France", "FRA", "250"));
			this.Children.Add(new Country("GF", "FRENCH GUIANA", "French Guiana", "GUF", "254"));
			this.Children.Add(new Country("PF", "FRENCH POLYNESIA", "French Polynesia", "PYF", "258"));
			this.Children.Add(new Country("TF", "FRENCH SOUTHERN TERRITORIES", "French Southern Territories", "ATF", "260"));
			this.Children.Add(new Country("GA", "GABON", "Gabon", "GAB", "266"));
			this.Children.Add(new Country("GM", "GAMBIA", "Gambia", "GMB", "270"));
			this.Children.Add(new Country("GE", "GEORGIA", "Georgia", "GEO", "268"));
			this.Children.Add(new Country("DE", "GERMANY", "Germany", "DEU", "276"));
			this.Children.Add(new Country("GH", "GHANA", "Ghana", "GHA", "288"));
			this.Children.Add(new Country("GI", "GIBRALTAR", "Gibraltar", "GIB", "292"));
			this.Children.Add(new Country("GR", "GREECE", "Greece", "GRC", "300"));
			this.Children.Add(new Country("GL", "GREENLAND", "Greenland", "GRL", "304"));
			this.Children.Add(new Country("GD", "GRENADA", "Grenada", "GRD", "308"));
			this.Children.Add(new Country("GP", "GUADELOUPE", "Guadeloupe", "GLP", "312"));
			this.Children.Add(new Country("GU", "GUAM", "Guam", "GUM", "316"));
			this.Children.Add(new Country("GT", "GUATEMALA", "Guatemala", "GTM", "320"));
			this.Children.Add(new Country("GN", "GUINEA", "Guinea", "GIN", "324"));
			this.Children.Add(new Country("GW", "GUINEA-BISSAU", "Guinea-Bissau", "GNB", "624"));
			this.Children.Add(new Country("GY", "GUYANA", "Guyana", "GUY", "328"));
			this.Children.Add(new Country("HT", "HAITI", "Haiti", "HTI", "332"));
			this.Children.Add(new Country("HM", "HEARD ISLAND AND MCDONALD ISLANDS", "Heard Island and Mcdonald Islands", "HMD", "334"));
			this.Children.Add(new Country("VA", "HOLY SEE (VATICAN CITY STATE))", "Holy See (Vatican City State))", "VAT", "336"));
			this.Children.Add(new Country("HN", "HONDURAS", "Honduras", "HND", "340"));
			this.Children.Add(new Country("HK", "HONG KONG", "Hong Kong", "HKG", "344"));
			this.Children.Add(new Country("HU", "HUNGARY", "Hungary", "HUN", "348"));
			this.Children.Add(new Country("IS", "ICELAND", "Iceland", "ISL", "352"));
			this.Children.Add(new Country("IN", "INDIA", "India", "IND", "356"));
			this.Children.Add(new Country("ID", "INDONESIA", "Indonesia", "IDN", "360"));
			this.Children.Add(new Country("IR", "IRAN, ISLAMIC REPUBLIC OF", "Iran, Islamic Republic of", "IRN", "364"));
			this.Children.Add(new Country("IQ", "IRAQ", "Iraq", "IRQ", "368"));
			this.Children.Add(new Country("IE", "IRELAND", "Ireland", "IRL", "372"));
			this.Children.Add(new Country("IL", "ISRAEL", "Israel", "ISR", "376"));
			this.Children.Add(new Country("IT", "ITALY", "Italy", "ITA", "380"));
			this.Children.Add(new Country("JM", "JAMAICA", "Jamaica", "JAM", "388"));
			this.Children.Add(new Country("JP", "JAPAN", "Japan", "JPN", "392"));
			this.Children.Add(new Country("JO", "JORDAN", "Jordan", "JOR", "400"));
			this.Children.Add(new Country("KZ", "KAZAKHSTAN", "Kazakhstan", "KAZ", "398"));
			this.Children.Add(new Country("KE", "KENYA", "Kenya", "KEN", "404"));
			this.Children.Add(new Country("KI", "KIRIBATI", "Kiribati", "KIR", "296"));
			this.Children.Add(new Country("KP", "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF", "Korea, Democratic People's Republic of", "PRK", "408"));
			this.Children.Add(new Country("KR", "KOREA, REPUBLIC OF", "Korea, Republic of", "KOR", "410"));
			this.Children.Add(new Country("KW", "KUWAIT", "Kuwait", "KWT", "414"));
			this.Children.Add(new Country("KG", "KYRGYZSTAN", "Kyrgyzstan", "KGZ", "417"));
			this.Children.Add(new Country("LA", "LAO PEOPLE'S DEMOCRATIC REPUBLIC", "Lao People's Democratic Republic", "LAO", "418"));
			this.Children.Add(new Country("LV", "LATVIA", "Latvia", "LVA", "428"));
			this.Children.Add(new Country("LB", "LEBANON", "Lebanon", "LBN", "422"));
			this.Children.Add(new Country("LS", "LESOTHO", "Lesotho", "LSO", "426"));
			this.Children.Add(new Country("LR", "LIBERIA", "Liberia", "LBR", "430"));
			this.Children.Add(new Country("LY", "LIBYAN ARAB JAMAHIRIYA", "Libyan Arab Jamahiriya", "LBY", "434"));
			this.Children.Add(new Country("LI", "LIECHTENSTEIN", "Liechtenstein", "LIE", "438"));
			this.Children.Add(new Country("LT", "LITHUANIA", "Lithuania", "LTU", "440"));
			this.Children.Add(new Country("LU", "LUXEMBOURG", "Luxembourg", "LUX", "442"));
			this.Children.Add(new Country("MO", "MACAO", "Macao", "MAC", "446"));
			this.Children.Add(new Country("MK", "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF", "Macedonia, the Former Yugoslav Republic of", "MKD", "807"));
			this.Children.Add(new Country("MG", "MADAGASCAR", "Madagascar", "MDG", "450"));
			this.Children.Add(new Country("MW", "MALAWI", "Malawi", "MWI", "454"));
			this.Children.Add(new Country("MY", "MALAYSIA", "Malaysia", "MYS", "458"));
			this.Children.Add(new Country("MV", "MALDIVES", "Maldives", "MDV", "462"));
			this.Children.Add(new Country("ML", "MALI", "Mali", "MLI", "466"));
			this.Children.Add(new Country("MT", "MALTA", "Malta", "MLT", "470"));
			this.Children.Add(new Country("MH", "MARSHALL ISLANDS", "Marshall Islands", "MHL", "584"));
			this.Children.Add(new Country("MQ", "MARTINIQUE", "Martinique", "MTQ", "474"));
			this.Children.Add(new Country("MR", "MAURITANIA", "Mauritania", "MRT", "478"));
			this.Children.Add(new Country("MU", "MAURITIUS", "Mauritius", "MUS", "480"));
			this.Children.Add(new Country("YT", "MAYOTTE", "Mayotte", "MYT", "175"));
			this.Children.Add(new Country("MX", "MEXICO", "Mexico", "MEX", "484"));
			this.Children.Add(new Country("FM", "MICRONESIA, FEDERATED STATES OF", "Micronesia, Federated States of", "FSM", "583"));
			this.Children.Add(new Country("MD", "MOLDOVA, REPUBLIC OF", "Moldova, Republic of", "MDA", "498"));
			this.Children.Add(new Country("MC", "MONACO", "Monaco", "MCO", "492"));
			this.Children.Add(new Country("MN", "MONGOLIA", "Mongolia", "MNG", "496"));
			this.Children.Add(new Country("ME", "MONTENEGRO", "Montenegro", "MNE", "499"));
			this.Children.Add(new Country("MS", "MONTSERRAT", "Montserrat", "MSR", "500"));
			this.Children.Add(new Country("MA", "MOROCCO", "Morocco", "MAR", "504"));
			this.Children.Add(new Country("MZ", "MOZAMBIQUE", "Mozambique", "MOZ", "508"));
			this.Children.Add(new Country("MM", "MYANMAR", "Myanmar", "MMR", "104"));
			this.Children.Add(new Country("NA", "NAMIBIA", "Namibia", "NAM", "516"));
			this.Children.Add(new Country("NR", "NAURU", "Nauru", "NRU", "520"));
			this.Children.Add(new Country("NP", "NEPAL", "Nepal", "NPL", "524"));
			this.Children.Add(new Country("NL", "NETHERLANDS", "Netherlands", "NLD", "528"));
			this.Children.Add(new Country("AN", "NETHERLANDS ANTILLES", "Netherlands Antilles", "ANT", "530"));
			this.Children.Add(new Country("NC", "NEW CALEDONIA", "New Caledonia", "NCL", "540"));
			this.Children.Add(new Country("NZ", "NEW ZEALAND", "New Zealand", "NZL", "554"));
			this.Children.Add(new Country("NI", "NICARAGUA", "Nicaragua", "NIC", "558"));
			this.Children.Add(new Country("NE", "NIGER", "Niger", "NER", "562"));
			this.Children.Add(new Country("NG", "NIGERIA", "Nigeria", "NGA", "566"));
			this.Children.Add(new Country("NU", "NIUE", "Niue", "NIU", "570"));
			this.Children.Add(new Country("NF", "NORFOLK ISLAND", "Norfolk Island", "NFK", "574"));
			this.Children.Add(new Country("MP", "NORTHERN MARIANA ISLANDS", "Northern Mariana Islands", "MNP", "580"));
			this.Children.Add(new Country("NO", "NORWAY", "Norway", "NOR", "578"));
			this.Children.Add(new Country("OM", "OMAN", "Oman", "OMN", "512"));
			this.Children.Add(new Country("PK", "PAKISTAN", "Pakistan", "PAK", "586"));
			this.Children.Add(new Country("PW", "PALAU", "Palau", "PLW", "585"));
			this.Children.Add(new Country("PS", "PALESTINIAN TERRITORY, OCCUPIED", "Palestinian Territory, Occupied", "PSE", "275"));
			this.Children.Add(new Country("PA", "PANAMA", "Panama", "PAN", "591"));
			this.Children.Add(new Country("PG", "PAPUA NEW GUINEA", "Papua New Guinea", "PNG", "598"));
			this.Children.Add(new Country("PY", "PARAGUAY", "Paraguay", "PRY", "600"));
			this.Children.Add(new Country("PE", "PERU", "Peru", "PER", "604"));
			this.Children.Add(new Country("PH", "PHILIPPINES", "Philippines", "PHL", "608"));
			this.Children.Add(new Country("PN", "PITCAIRN", "Pitcairn", "PCN", "612"));
			this.Children.Add(new Country("PL", "POLAND", "Poland", "POL", "616"));
			this.Children.Add(new Country("PT", "PORTUGAL", "Portugal", "PRT", "620"));
			this.Children.Add(new Country("PR", "PUERTO RICO", "Puerto Rico", "PRI", "630"));
			this.Children.Add(new Country("QA", "QATAR", "Qatar", "QAT", "634"));
			this.Children.Add(new Country("RE", "REUNION", "Reunion", "REU", "638"));
			this.Children.Add(new Country("RO", "ROMANIA", "Romania", "ROM", "642"));
			this.Children.Add(new Country("RU", "RUSSIAN FEDERATION", "Russian Federation", "RUS", "643"));
			this.Children.Add(new Country("RW", "RWANDA", "Rwanda", "RWA", "646"));
			this.Children.Add(new Country("SH", "SAINT HELENA", "Saint Helena", "SHN", "654"));
			this.Children.Add(new Country("KN", "SAINT KITTS AND NEVIS", "Saint Kitts and Nevis", "KNA", "659"));
			this.Children.Add(new Country("LC", "SAINT LUCIA", "Saint Lucia", "LCA", "662"));
			this.Children.Add(new Country("PM", "SAINT PIERRE AND MIQUELON", "Saint Pierre and Miquelon", "SPM", "666"));
			this.Children.Add(new Country("VC", "SAINT VINCENT AND THE GRENADINES", "Saint Vincent and the Grenadines", "VCT", "670"));
			this.Children.Add(new Country("WS", "SAMOA", "Samoa", "WSM", "882"));
			this.Children.Add(new Country("SM", "SAN MARINO", "San Marino", "SMR", "674"));
			this.Children.Add(new Country("ST", "SAO TOME AND PRINCIPE", "Sao Tome and Principe", "STP", "678"));
			this.Children.Add(new Country("SA", "SAUDI ARABIA", "Saudi Arabia", "SAU", "682"));
			this.Children.Add(new Country("SN", "SENEGAL", "Senegal", "SEN", "686"));
			this.Children.Add(new Country("RS", "SERBIA", "Serbia", "SRB", "688"));
			this.Children.Add(new Country("SC", "SEYCHELLES", "Seychelles", "SYC", "690"));
			this.Children.Add(new Country("SL", "SIERRA LEONE", "Sierra Leone", "SLE", "694"));
			this.Children.Add(new Country("SG", "SINGAPORE", "Singapore", "SGP", "702"));
			this.Children.Add(new Country("SK", "SLOVAKIA", "Slovakia", "SVK", "703"));
			this.Children.Add(new Country("SI", "SLOVENIA", "Slovenia", "SVN", "705"));
			this.Children.Add(new Country("SB", "SOLOMON ISLANDS", "Solomon Islands", "SLB", "090"));
			this.Children.Add(new Country("SO", "SOMALIA", "Somalia", "SOM", "706"));
			this.Children.Add(new Country("ZA", "SOUTH AFRICA", "South Africa", "ZAF", "710"));
			this.Children.Add(new Country("GS", "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS", "South Georgia and the South Sandwich Islands", "SGS", "239"));
			this.Children.Add(new Country("ES", "SPAIN", "Spain", "ESP", "724"));
			this.Children.Add(new Country("LK", "SRI LANKA", "Sri Lanka", "LKA", "144"));
			this.Children.Add(new Country("SD", "SUDAN", "Sudan", "SDN", "736"));
			this.Children.Add(new Country("SR", "SURINAME", "Suriname", "SUR", "740"));
			this.Children.Add(new Country("SJ", "SVALBARD AND JAN MAYEN", "Svalbard and Jan Mayen", "SJM", "744"));
			this.Children.Add(new Country("SZ", "SWAZILAND", "Swaziland", "SWZ", "748"));
			this.Children.Add(new Country("SE", "SWEDEN", "Sweden", "SWE", "752"));
			this.Children.Add(new Country("CH", "SWITZERLAND", "Switzerland", "CHE", "756"));
			this.Children.Add(new Country("SY", "SYRIAN ARAB REPUBLIC", "Syrian Arab Republic", "SYR", "760"));
			this.Children.Add(new Country("TW", "TAIWAN, PROVINCE OF CHINA", "Taiwan, Province of China", "TWN", "158"));
			this.Children.Add(new Country("TJ", "TAJIKISTAN", "Tajikistan", "TJK", "762"));
			this.Children.Add(new Country("TZ", "TANZANIA, UNITED REPUBLIC OF", "Tanzania, United Republic of", "TZA", "834"));
			this.Children.Add(new Country("TH", "THAILAND", "Thailand", "THA", "764"));
			this.Children.Add(new Country("TL", "TIMOR-LESTE", "Timor-Leste", "TLS", "626"));
			this.Children.Add(new Country("TG", "TOGO", "Togo", "TGO", "768"));
			this.Children.Add(new Country("TK", "TOKELAU", "Tokelau", "TKL", "772"));
			this.Children.Add(new Country("TO", "TONGA", "Tonga", "TON", "776"));
			this.Children.Add(new Country("TT", "TRINIDAD AND TOBAGO", "Trinidad and Tobago", "TTO", "780"));
			this.Children.Add(new Country("TN", "TUNISIA", "Tunisia", "TUN", "788"));
			this.Children.Add(new Country("TR", "TURKEY", "Turkey", "TUR", "792"));
			this.Children.Add(new Country("TM", "TURKMENISTAN", "Turkmenistan", "TKM", "795"));
			this.Children.Add(new Country("TC", "TURKS AND CAICOS ISLANDS", "Turks and Caicos Islands", "TCA", "796"));
			this.Children.Add(new Country("TV", "TUVALU", "Tuvalu", "TUV", "798"));
			this.Children.Add(new Country("UG", "UGANDA", "Uganda", "UGA", "800"));
			this.Children.Add(new Country("UA", "UKRAINE", "Ukraine", "UKR", "804"));
			this.Children.Add(new Country("AE", "UNITED ARAB EMIRATES", "United Arab Emirates", "ARE", "784"));
			this.Children.Add(new Country("GB", "UNITED KINGDOM", "United Kingdom", "GBR", "826"));
			this.Children.Add(new Country("US", "UNITED STATES", "United States", "USA", "840"));
			this.Children.Add(new Country("UM", "UNITED STATES MINOR OUTLYING ISLANDS", "United States Minor Outlying Islands", "UMI", "581"));
			this.Children.Add(new Country("UY", "URUGUAY", "Uruguay", "URY", "858"));
			this.Children.Add(new Country("UZ", "UZBEKISTAN", "Uzbekistan", "UZB", "860"));
			this.Children.Add(new Country("VU", "VANUATU", "Vanuatu", "VUT", "548"));
			this.Children.Add(new Country("VE", "VENEZUELA", "Venezuela", "VEN", "862"));
			this.Children.Add(new Country("VN", "VIET NAM", "Viet Nam", "VNM", "704"));
			this.Children.Add(new Country("VG", "VIRGIN ISLANDS, BRITISH", "Virgin Islands, British", "VGB", "092"));
			this.Children.Add(new Country("VI", "VIRGIN ISLANDS, U.S.", "Virgin Islands, U.s.", "VIR", "850"));
			this.Children.Add(new Country("WF", "WALLIS AND FUTUNA", "Wallis and Futuna", "WLF", "876"));
			this.Children.Add(new Country("EH", "WESTERN SAHARA", "Western Sahara", "ESH", "732"));
			this.Children.Add(new Country("YE", "YEMEN", "Yemen", "YEM", "887"));
			this.Children.Add(new Country("ZM", "ZAMBIA", "Zambia", "ZMB", "894"));
			this.Children.Add(new Country("ZW", "ZIMBABWE", "Zimbabwe", "ZWE", "716"));

			foreach (Country country in this.Children)
				country.Parent = this;
		}
	}
}
