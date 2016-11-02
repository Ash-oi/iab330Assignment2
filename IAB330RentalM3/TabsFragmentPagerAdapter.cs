using System;
using Android.Support.V4.App;
using Java.Lang;

namespace IAB330RentalM3
{
    ///<summary>
    /// this is mostly copied from here:
    /// http://www.appliedcodelog.com/2016/05/material-design-tab-in-xamarin-android.html
    /// original author: Suchith Madavu 
    ///</<summary>
    public class TabsFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly Fragment[] fragments;

        private readonly ICharSequence[] titles;

        public TabsFragmentPagerAdapter(FragmentManager fm, Fragment[] fragments, ICharSequence[] titles) : base(fm)
        {
            this.fragments = fragments;
            this.titles = titles;
        }
        public override int Count
        {
            get
            {
                return fragments.Length;
            }
        }

        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return titles[position];
        }
    }
}
