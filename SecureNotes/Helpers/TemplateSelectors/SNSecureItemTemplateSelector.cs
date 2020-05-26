using SecureNotes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SecureNotes.Helpers.TemplateSelectors
{

    public class SNSecureItemTemplateSelector : DataTemplateSelector
    {

        public DataTemplate AlarmCodeTemplate { get; set; }
        public DataTemplate DriversLicenseTemplate { get; set; }
        public DataTemplate EstatePlanTemplate { get; set; }
        public DataTemplate FrequentFlyerTemplate { get; set; }
        public DataTemplate NoteSecureTemplate { get; set; }
        public DataTemplate HealthInsuranceTemplate { get; set; }
        public DataTemplate HotelRewardsTemplate { get; set; }
        public DataTemplate InsuranceTemplate { get; set; }
        public DataTemplate MemberIDTemplate { get; set; }
        public DataTemplate PassportCodeTemplate { get; set; }
        public DataTemplate PrescriptionTemplate { get; set; }
        public DataTemplate SocialSecurityTemplate { get; set; }
        public DataTemplate SoftwareLicenseTemplate { get; set; }

        public SNSecureItemTemplateSelector()
        {
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is AlarmCodeSecureItemViewModel)
                return AlarmCodeTemplate;

            if (item is DriversLicenseSecureItemViewModel)
                return DriversLicenseTemplate;

            if (item is EstatePlanSecureItemViewModel)
                return EstatePlanTemplate;

            if (item is FrequentFlyerSecureItemViewModel)
                return FrequentFlyerTemplate;

            if (item is NoteSecureItemViewModel)
                return NoteSecureTemplate;

            if (item is HealthInsuranceSecureItemViewModel)
                return HealthInsuranceTemplate;

            if (item is HotelRewardsSecureItemViewModel)
                return HotelRewardsTemplate;

            if (item is InsuranceSecureItemViewModel)
                return InsuranceTemplate;
            
            if (item is MemberIDSecureItemViewModel)
                return MemberIDTemplate;

            if (item is PassportCodeSecureItemViewModel)
                return PassportCodeTemplate;

            if (item is PrescriptionSecureItemViewModel)
                return PrescriptionTemplate;

            if (item is SocialSecuritySecureItemViewModel)
                return SocialSecurityTemplate;

            if (item is SoftwareLicenseSecureItemViewModel)
                return SoftwareLicenseTemplate;

            return AlarmCodeTemplate;

        }
    }
}
