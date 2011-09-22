using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Country List", Description = "Container for countries")]
	[RestrictParents(typeof(ReferenceDataNode))]
	public class CountryList : BaseContentItem, ISelfPopulator
	{
		public CountryList()
		{
			Name = "countries";
			Title = "Countries";
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.World); }
		}

		#region ISelfPopulator Members

		public virtual void Populate()
		{
			Children.Add(new Country("AF", "AFGHANISTAN", "Afghanistan", "AFG", "004"));
            Children.Add(new Country("AX", "ALAND ISLANDS", "Aland Islands", "ALA", "000"));
            Children.Add(new Country("AL", "ALBANIA", "Albania", "ALB", "008"));
			Children.Add(new Country("DZ", "ALGERIA", "Algeria", "DZA", "012"));
			Children.Add(new Country("AS", "AMERICAN SAMOA", "American Samoa", "ASM", "016"));
			Children.Add(new Country("AD", "ANDORRA", "Andorra", "AND", "020"));
			Children.Add(new Country("AO", "ANGOLA", "Angola", "AGO", "024"));
			Children.Add(new Country("AI", "ANGUILLA", "Anguilla", "AIA", "660"));
			Children.Add(new Country("AQ", "ANTARCTICA", "Antarctica", "ATA", "010"));
			Children.Add(new Country("AG", "ANTIGUA AND BARBUDA", "Antigua and Barbuda", "ATG", "028"));
			Children.Add(new Country("AR", "ARGENTINA", "Argentina", "ARG", "032"));
			Children.Add(new Country("AM", "ARMENIA", "Armenia", "ARM", "051"));
			Children.Add(new Country("AW", "ARUBA", "Aruba", "ABW", "533"));
			Children.Add(new Country("AU", "AUSTRALIA", "Australia", "AUS", "036"));
			Children.Add(new Country("AT", "AUSTRIA", "Austria", "AUT", "040"));
			Children.Add(new Country("AZ", "AZERBAIJAN", "Azerbaijan", "AZE", "031"));

			Children.Add(new Country("BS", "BAHAMAS", "Bahamas", "BHS", "044"));
			Children.Add(new Country("BH", "BAHRAIN", "Bahrain", "BHR", "048"));
			Children.Add(new Country("BD", "BANGLADESH", "Bangladesh", "BGD", "050"));
			Children.Add(new Country("BB", "BARBADOS", "Barbados", "BRB", "052"));
			Children.Add(new Country("BY", "BELARUS", "Belarus", "BLR", "112"));
			Children.Add(new Country("BE", "BELGIUM", "Belgium", "BEL", "056"));
			Children.Add(new Country("BZ", "BELIZE", "Belize", "BLZ", "084"));
			Children.Add(new Country("BJ", "BENIN", "Benin", "BEN", "204"));
			Children.Add(new Country("BM", "BERMUDA", "Bermuda", "BMU", "060"));
			Children.Add(new Country("BT", "BHUTAN", "Bhutan", "BTN", "064"));
			Children.Add(new Country("BO", "BOLIVIA", "Bolivia", "BOL", "068"));
            Children.Add(new Country("BQ", "BONAIRE, SAINT EUSTATIUS AND SABA", "Bonaire, Saint Eustatius And Saba", "BES", "000"));
			Children.Add(new Country("BA", "BOSNIA AND HERZEGOVINA", "Bosnia and Herzegovina", "BIH", "070"));
			Children.Add(new Country("BW", "BOTSWANA", "Botswana", "BWA", "072"));
			Children.Add(new Country("BV", "BOUVET ISLAND", "Bouvet Island", "BVT", "074"));
			Children.Add(new Country("BR", "BRAZIL", "Brazil", "BRA", "076"));
			Children.Add(new Country("IO", "BRITISH INDIAN OCEAN TERRITORY", "British Indian Ocean Territory", "IOT", "086"));
			Children.Add(new Country("BN", "BRUNEI DARUSSALAM", "Brunei Darussalam", "BRN", "096"));
			Children.Add(new Country("BG", "BULGARIA", "Bulgaria", "BGR", "100"));
			Children.Add(new Country("BF", "BURKINA FASO", "Burkina Faso", "BFA", "854"));
			Children.Add(new Country("BI", "BURUNDI", "Burundi", "BDI", "108"));

			Children.Add(new Country("KH", "CAMBODIA", "Cambodia", "KHM", "116"));
			Children.Add(new Country("CM", "CAMEROON", "Cameroon", "CMR", "120"));
			Children.Add(new Country("CA", "CANADA", "Canada", "CAN", "124"));
			Children.Add(new Country("CV", "CAPE VERDE", "Cape Verde", "CPV", "132"));
			Children.Add(new Country("KY", "CAYMAN ISLANDS", "Cayman Islands", "CYM", "136"));
			Children.Add(new Country("CF", "CENTRAL AFRICAN REPUBLIC", "Central African Republic", "CAF", "140"));
			Children.Add(new Country("TD", "CHAD", "Chad", "TCD", "148"));
			Children.Add(new Country("CL", "CHILE", "Chile", "CHL", "152"));
			Children.Add(new Country("CN", "CHINA", "China", "CHN", "156"));
			Children.Add(new Country("CX", "CHRISTMAS ISLAND", "Christmas Island", "CXR", "162"));
			Children.Add(new Country("CC", "COCOS (KEELING)) ISLANDS", "Cocos (Keeling)) Islands", "CCK", "166"));
			Children.Add(new Country("CO", "COLOMBIA", "Colombia", "COL", "170"));
			Children.Add(new Country("KM", "COMOROS", "Comoros", "COM", "174"));
			Children.Add(new Country("CG", "CONGO", "Congo", "COG", "178"));
			Children.Add(new Country("CD", "CONGO, THE DEMOCRATIC REPUBLIC OF THE", "Congo, the Democratic Republic of the", "COD", "180"));
			Children.Add(new Country("CK", "COOK ISLANDS", "Cook Islands", "COK", "184"));
			Children.Add(new Country("CR", "COSTA RICA", "Costa Rica", "CRI", "188"));
			Children.Add(new Country("CI", "COTE D'IVOIRE", "Cote D'Ivoire", "CIV", "384"));
			Children.Add(new Country("HR", "CROATIA", "Croatia", "HRV", "191"));
			Children.Add(new Country("CU", "CUBA", "Cuba", "CUB", "192"));
            Children.Add(new Country("CW", "CURACAO", "Curacao", "CUW", "000"));
            Children.Add(new Country("CY", "CYPRUS", "Cyprus", "CYP", "196"));
			Children.Add(new Country("CZ", "CZECH REPUBLIC", "Czech Republic", "CZE", "203"));
			
            Children.Add(new Country("DK", "DENMARK", "Denmark", "DNK", "208"));
			Children.Add(new Country("DJ", "DJIBOUTI", "Djibouti", "DJI", "262"));
			Children.Add(new Country("DM", "DOMINICA", "Dominica", "DMA", "212"));
			Children.Add(new Country("DO", "DOMINICAN REPUBLIC", "Dominican Republic", "DOM", "214"));
			
            Children.Add(new Country("EC", "ECUADOR", "Ecuador", "ECU", "218"));
			Children.Add(new Country("EG", "EGYPT", "Egypt", "EGY", "818"));
			Children.Add(new Country("SV", "EL SALVADOR", "El Salvador", "SLV", "222"));
			Children.Add(new Country("GQ", "EQUATORIAL GUINEA", "Equatorial Guinea", "GNQ", "226"));
			Children.Add(new Country("ER", "ERITREA", "Eritrea", "ERI", "232"));
			Children.Add(new Country("EE", "ESTONIA", "Estonia", "EST", "233"));
			Children.Add(new Country("ET", "ETHIOPIA", "Ethiopia", "ETH", "231"));

			Children.Add(new Country("FK", "FALKLAND ISLANDS (MALVINAS))", "Falkland Islands (Malvinas))", "FLK", "238"));
			Children.Add(new Country("FO", "FAROE ISLANDS", "Faroe Islands", "FRO", "234"));
			Children.Add(new Country("FJ", "FIJI", "Fiji", "FJI", "242"));
			Children.Add(new Country("FI", "FINLAND", "Finland", "FIN", "246"));
			Children.Add(new Country("FR", "FRANCE", "France", "FRA", "250"));
			Children.Add(new Country("GF", "FRENCH GUIANA", "French Guiana", "GUF", "254"));
			Children.Add(new Country("PF", "FRENCH POLYNESIA", "French Polynesia", "PYF", "258"));
			Children.Add(new Country("TF", "FRENCH SOUTHERN TERRITORIES", "French Southern Territories", "ATF", "260"));
			
            Children.Add(new Country("GA", "GABON", "Gabon", "GAB", "266"));
			Children.Add(new Country("GM", "GAMBIA", "Gambia", "GMB", "270"));
			Children.Add(new Country("GE", "GEORGIA", "Georgia", "GEO", "268"));
			Children.Add(new Country("DE", "GERMANY", "Germany", "DEU", "276"));
			Children.Add(new Country("GH", "GHANA", "Ghana", "GHA", "288"));
			Children.Add(new Country("GI", "GIBRALTAR", "Gibraltar", "GIB", "292"));
			Children.Add(new Country("GR", "GREECE", "Greece", "GRC", "300"));
			Children.Add(new Country("GL", "GREENLAND", "Greenland", "GRL", "304"));
			Children.Add(new Country("GD", "GRENADA", "Grenada", "GRD", "308"));
			Children.Add(new Country("GP", "GUADELOUPE", "Guadeloupe", "GLP", "312"));
			Children.Add(new Country("GU", "GUAM", "Guam", "GUM", "316"));
			Children.Add(new Country("GT", "GUATEMALA", "Guatemala", "GTM", "320"));
            Children.Add(new Country("GG", "GUERNSEY", "Guernsey", "GGY", "000"));
			Children.Add(new Country("GN", "GUINEA", "Guinea", "GIN", "324"));
			Children.Add(new Country("GW", "GUINEA-BISSAU", "Guinea-Bissau", "GNB", "624"));
			Children.Add(new Country("GY", "GUYANA", "Guyana", "GUY", "328"));

			Children.Add(new Country("HT", "HAITI", "Haiti", "HTI", "332"));
			Children.Add(new Country("HM", "HEARD ISLAND AND MCDONALD ISLANDS", "Heard Island and Mcdonald Islands", "HMD", "334"));
			Children.Add(new Country("VA", "HOLY SEE (VATICAN CITY STATE))", "Holy See (Vatican City State))", "VAT", "336"));
			Children.Add(new Country("HN", "HONDURAS", "Honduras", "HND", "340"));
			Children.Add(new Country("HK", "HONG KONG", "Hong Kong", "HKG", "344"));
			Children.Add(new Country("HU", "HUNGARY", "Hungary", "HUN", "348"));

			Children.Add(new Country("IS", "ICELAND", "Iceland", "ISL", "352"));
			Children.Add(new Country("IN", "INDIA", "India", "IND", "356"));
			Children.Add(new Country("ID", "INDONESIA", "Indonesia", "IDN", "360"));
			Children.Add(new Country("IR", "IRAN, ISLAMIC REPUBLIC OF", "Iran, Islamic Republic of", "IRN", "364"));
			Children.Add(new Country("IQ", "IRAQ", "Iraq", "IRQ", "368"));
			Children.Add(new Country("IE", "IRELAND", "Ireland", "IRL", "372"));
            Children.Add(new Country("IM", "ISLE OF MAN", "Isle Of Man", "IMN", "000"));
            Children.Add(new Country("IL", "ISRAEL", "Israel", "ISR", "376"));
			Children.Add(new Country("IT", "ITALY", "Italy", "ITA", "380"));

			Children.Add(new Country("JM", "JAMAICA", "Jamaica", "JAM", "388"));
			Children.Add(new Country("JP", "JAPAN", "Japan", "JPN", "392"));
            Children.Add(new Country("JE", "JERSEY", "Jersey", "JEY", "000"));
            Children.Add(new Country("JO", "JORDAN", "Jordan", "JOR", "400"));

			Children.Add(new Country("KZ", "KAZAKHSTAN", "Kazakhstan", "KAZ", "398"));
			Children.Add(new Country("KE", "KENYA", "Kenya", "KEN", "404"));
			Children.Add(new Country("KI", "KIRIBATI", "Kiribati", "KIR", "296"));
			Children.Add(new Country("KP", "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF", "Korea, Democratic People's Republic of", "PRK", "408"));
			Children.Add(new Country("KR", "KOREA, REPUBLIC OF", "Korea, Republic of", "KOR", "410"));
			Children.Add(new Country("KW", "KUWAIT", "Kuwait", "KWT", "414"));
			Children.Add(new Country("KG", "KYRGYZSTAN", "Kyrgyzstan", "KGZ", "417"));

			Children.Add(new Country("LA", "LAO PEOPLE'S DEMOCRATIC REPUBLIC", "Lao People's Democratic Republic", "LAO", "418"));
			Children.Add(new Country("LV", "LATVIA", "Latvia", "LVA", "428"));
			Children.Add(new Country("LB", "LEBANON", "Lebanon", "LBN", "422"));
			Children.Add(new Country("LS", "LESOTHO", "Lesotho", "LSO", "426"));
			Children.Add(new Country("LR", "LIBERIA", "Liberia", "LBR", "430"));
			Children.Add(new Country("LY", "LIBYAN ARAB JAMAHIRIYA", "Libyan Arab Jamahiriya", "LBY", "434"));
			Children.Add(new Country("LI", "LIECHTENSTEIN", "Liechtenstein", "LIE", "438"));
			Children.Add(new Country("LT", "LITHUANIA", "Lithuania", "LTU", "440"));
			Children.Add(new Country("LU", "LUXEMBOURG", "Luxembourg", "LUX", "442"));

			Children.Add(new Country("MO", "MACAO", "Macao", "MAC", "446"));
			Children.Add(new Country("MK", "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF", "Macedonia, the Former Yugoslav Republic of", "MKD", "807"));
			Children.Add(new Country("MG", "MADAGASCAR", "Madagascar", "MDG", "450"));
			Children.Add(new Country("MW", "MALAWI", "Malawi", "MWI", "454"));
			Children.Add(new Country("MY", "MALAYSIA", "Malaysia", "MYS", "458"));
			Children.Add(new Country("MV", "MALDIVES", "Maldives", "MDV", "462"));
			Children.Add(new Country("ML", "MALI", "Mali", "MLI", "466"));
			Children.Add(new Country("MT", "MALTA", "Malta", "MLT", "470"));
			Children.Add(new Country("MH", "MARSHALL ISLANDS", "Marshall Islands", "MHL", "584"));
			Children.Add(new Country("MQ", "MARTINIQUE", "Martinique", "MTQ", "474"));
			Children.Add(new Country("MR", "MAURITANIA", "Mauritania", "MRT", "478"));
			Children.Add(new Country("MU", "MAURITIUS", "Mauritius", "MUS", "480"));
			Children.Add(new Country("YT", "MAYOTTE", "Mayotte", "MYT", "175"));
			Children.Add(new Country("MX", "MEXICO", "Mexico", "MEX", "484"));
			Children.Add(new Country("FM", "MICRONESIA, FEDERATED STATES OF", "Micronesia, Federated States of", "FSM", "583"));
			Children.Add(new Country("MD", "MOLDOVA, REPUBLIC OF", "Moldova, Republic of", "MDA", "498"));
			Children.Add(new Country("MC", "MONACO", "Monaco", "MCO", "492"));
			Children.Add(new Country("MN", "MONGOLIA", "Mongolia", "MNG", "496"));
			Children.Add(new Country("ME", "MONTENEGRO", "Montenegro", "MNE", "499"));
			Children.Add(new Country("MS", "MONTSERRAT", "Montserrat", "MSR", "500"));
			Children.Add(new Country("MA", "MOROCCO", "Morocco", "MAR", "504"));
			Children.Add(new Country("MZ", "MOZAMBIQUE", "Mozambique", "MOZ", "508"));
			Children.Add(new Country("MM", "MYANMAR", "Myanmar", "MMR", "104"));
                
			Children.Add(new Country("NA", "NAMIBIA", "Namibia", "NAM", "516"));
			Children.Add(new Country("NR", "NAURU", "Nauru", "NRU", "520"));
			Children.Add(new Country("NP", "NEPAL", "Nepal", "NPL", "524"));
			Children.Add(new Country("NL", "NETHERLANDS", "Netherlands", "NLD", "528"));
			Children.Add(new Country("NC", "NEW CALEDONIA", "New Caledonia", "NCL", "540"));
			Children.Add(new Country("NZ", "NEW ZEALAND", "New Zealand", "NZL", "554"));
			Children.Add(new Country("NI", "NICARAGUA", "Nicaragua", "NIC", "558"));
			Children.Add(new Country("NE", "NIGER", "Niger", "NER", "562"));
			Children.Add(new Country("NG", "NIGERIA", "Nigeria", "NGA", "566"));
			Children.Add(new Country("NU", "NIUE", "Niue", "NIU", "570"));
			Children.Add(new Country("NF", "NORFOLK ISLAND", "Norfolk Island", "NFK", "574"));
			Children.Add(new Country("MP", "NORTHERN MARIANA ISLANDS", "Northern Mariana Islands", "MNP", "580"));
			Children.Add(new Country("NO", "NORWAY", "Norway", "NOR", "578"));

			Children.Add(new Country("OM", "OMAN", "Oman", "OMN", "512"));

			Children.Add(new Country("PK", "PAKISTAN", "Pakistan", "PAK", "586"));
			Children.Add(new Country("PW", "PALAU", "Palau", "PLW", "585"));
			Children.Add(new Country("PS", "PALESTINIAN TERRITORY, OCCUPIED", "Palestinian Territory, Occupied", "PSE", "275"));
			Children.Add(new Country("PA", "PANAMA", "Panama", "PAN", "591"));
			Children.Add(new Country("PG", "PAPUA NEW GUINEA", "Papua New Guinea", "PNG", "598"));
			Children.Add(new Country("PY", "PARAGUAY", "Paraguay", "PRY", "600"));
			Children.Add(new Country("PE", "PERU", "Peru", "PER", "604"));
			Children.Add(new Country("PH", "PHILIPPINES", "Philippines", "PHL", "608"));
			Children.Add(new Country("PN", "PITCAIRN", "Pitcairn", "PCN", "612"));
			Children.Add(new Country("PL", "POLAND", "Poland", "POL", "616"));
			Children.Add(new Country("PT", "PORTUGAL", "Portugal", "PRT", "620"));
			Children.Add(new Country("PR", "PUERTO RICO", "Puerto Rico", "PRI", "630"));
			
            Children.Add(new Country("QA", "QATAR", "Qatar", "QAT", "634"));
			
            Children.Add(new Country("RE", "REUNION", "Reunion", "REU", "638"));
			Children.Add(new Country("RO", "ROMANIA", "Romania", "ROM", "642"));
			Children.Add(new Country("RU", "RUSSIAN FEDERATION", "Russian Federation", "RUS", "643"));
			Children.Add(new Country("RW", "RWANDA", "Rwanda", "RWA", "646"));

            Children.Add(new Country("BL", "SAINT BARTHELEMY", "Saint Barthelemy", "BLM", "000"));
            Children.Add(new Country("SH", "SAINT HELENA", "Saint Helena", "SHN", "654"));
            Children.Add(new Country("KN", "SAINT KITTS AND NEVIS", "Saint Kitts and Nevis", "KNA", "659"));
			Children.Add(new Country("LC", "SAINT LUCIA", "Saint Lucia", "LCA", "662"));
            Children.Add(new Country("MF", "SAINT MARTIN (FRENCH PART)", "Saint Martin (French Part)", "MAF", "663"));
			Children.Add(new Country("PM", "SAINT PIERRE AND MIQUELON", "Saint Pierre and Miquelon", "SPM", "666"));
			Children.Add(new Country("VC", "SAINT VINCENT AND THE GRENADINES", "Saint Vincent and the Grenadines", "VCT", "670"));
			Children.Add(new Country("WS", "SAMOA", "Samoa", "WSM", "882"));
			Children.Add(new Country("SM", "SAN MARINO", "San Marino", "SMR", "674"));
			Children.Add(new Country("ST", "SAO TOME AND PRINCIPE", "Sao Tome and Principe", "STP", "678"));
			Children.Add(new Country("SA", "SAUDI ARABIA", "Saudi Arabia", "SAU", "682"));
			Children.Add(new Country("SN", "SENEGAL", "Senegal", "SEN", "686"));
			Children.Add(new Country("RS", "SERBIA", "Serbia", "SRB", "688"));
			Children.Add(new Country("SC", "SEYCHELLES", "Seychelles", "SYC", "690"));
			Children.Add(new Country("SL", "SIERRA LEONE", "Sierra Leone", "SLE", "694"));
			Children.Add(new Country("SG", "SINGAPORE", "Singapore", "SGP", "702"));
            Children.Add(new Country("SX", "SINT MAARTEN (DUTCH PART)", "Sint Martin (Dutch Part)", "SXM", "703"));
            Children.Add(new Country("SK", "SLOVAKIA", "Slovakia", "SVK", "703"));  
			Children.Add(new Country("SI", "SLOVENIA", "Slovenia", "SVN", "705"));
			Children.Add(new Country("SB", "SOLOMON ISLANDS", "Solomon Islands", "SLB", "090"));
			Children.Add(new Country("SO", "SOMALIA", "Somalia", "SOM", "706"));
			Children.Add(new Country("ZA", "SOUTH AFRICA", "South Africa", "ZAF", "710"));
			Children.Add(new Country("GS", "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS", "South Georgia and the South Sandwich Islands", "SGS", "239"));
			Children.Add(new Country("ES", "SPAIN", "Spain", "ESP", "724"));
			Children.Add(new Country("LK", "SRI LANKA", "Sri Lanka", "LKA", "144"));
			Children.Add(new Country("SD", "SUDAN", "Sudan", "SDN", "736"));
			Children.Add(new Country("SR", "SURINAME", "Suriname", "SUR", "740"));
			Children.Add(new Country("SJ", "SVALBARD AND JAN MAYEN", "Svalbard and Jan Mayen", "SJM", "744"));
			Children.Add(new Country("SZ", "SWAZILAND", "Swaziland", "SWZ", "748"));
			Children.Add(new Country("SE", "SWEDEN", "Sweden", "SWE", "752"));
			Children.Add(new Country("CH", "SWITZERLAND", "Switzerland", "CHE", "756"));
			Children.Add(new Country("SY", "SYRIAN ARAB REPUBLIC", "Syrian Arab Republic", "SYR", "760"));
			
            Children.Add(new Country("TW", "TAIWAN, PROVINCE OF CHINA", "Taiwan, Province of China", "TWN", "158"));
			Children.Add(new Country("TJ", "TAJIKISTAN", "Tajikistan", "TJK", "762"));
			Children.Add(new Country("TZ", "TANZANIA, UNITED REPUBLIC OF", "Tanzania, United Republic of", "TZA", "834"));
			Children.Add(new Country("TH", "THAILAND", "Thailand", "THA", "764"));
			Children.Add(new Country("TL", "TIMOR-LESTE", "Timor-Leste", "TLS", "626"));
			Children.Add(new Country("TG", "TOGO", "Togo", "TGO", "768"));
			Children.Add(new Country("TK", "TOKELAU", "Tokelau", "TKL", "772"));
			Children.Add(new Country("TO", "TONGA", "Tonga", "TON", "776"));
			Children.Add(new Country("TT", "TRINIDAD AND TOBAGO", "Trinidad and Tobago", "TTO", "780"));
			Children.Add(new Country("TN", "TUNISIA", "Tunisia", "TUN", "788"));
			Children.Add(new Country("TR", "TURKEY", "Turkey", "TUR", "792"));
			Children.Add(new Country("TM", "TURKMENISTAN", "Turkmenistan", "TKM", "795"));
			Children.Add(new Country("TC", "TURKS AND CAICOS ISLANDS", "Turks and Caicos Islands", "TCA", "796"));
			Children.Add(new Country("TV", "TUVALU", "Tuvalu", "TUV", "798"));
			
            Children.Add(new Country("UG", "UGANDA", "Uganda", "UGA", "800"));
			Children.Add(new Country("UA", "UKRAINE", "Ukraine", "UKR", "804"));
			Children.Add(new Country("AE", "UNITED ARAB EMIRATES", "United Arab Emirates", "ARE", "784"));
			Children.Add(new Country("GB", "UNITED KINGDOM", "United Kingdom", "GBR", "826"));
			Children.Add(new Country("US", "UNITED STATES", "United States", "USA", "840"));
			Children.Add(new Country("UM", "UNITED STATES MINOR OUTLYING ISLANDS", "United States Minor Outlying Islands", "UMI", "581"));
			Children.Add(new Country("UY", "URUGUAY", "Uruguay", "URY", "858"));
			Children.Add(new Country("UZ", "UZBEKISTAN", "Uzbekistan", "UZB", "860"));
			
            Children.Add(new Country("VU", "VANUATU", "Vanuatu", "VUT", "548"));
			Children.Add(new Country("VE", "VENEZUELA", "Venezuela", "VEN", "862"));
			Children.Add(new Country("VN", "VIET NAM", "Viet Nam", "VNM", "704"));
			Children.Add(new Country("VG", "VIRGIN ISLANDS, BRITISH", "Virgin Islands, British", "VGB", "092"));
			Children.Add(new Country("VI", "VIRGIN ISLANDS, U.S.", "Virgin Islands, U.s.", "VIR", "850"));
			
            Children.Add(new Country("WF", "WALLIS AND FUTUNA", "Wallis and Futuna", "WLF", "876"));
			Children.Add(new Country("EH", "WESTERN SAHARA", "Western Sahara", "ESH", "732"));
			
            Children.Add(new Country("YE", "YEMEN", "Yemen", "YEM", "887"));
			
            Children.Add(new Country("ZM", "ZAMBIA", "Zambia", "ZMB", "894"));
			Children.Add(new Country("ZW", "ZIMBABWE", "Zimbabwe", "ZWE", "716"));

            Zeus.Context.Persister.Save(this);

            foreach (Country country in Children)
            {
                country.Parent = this;
                Zeus.Context.Persister.Save(country);
            }

            //Zeus.Context.Persister.Save(this);

		}

		#endregion
	}
}