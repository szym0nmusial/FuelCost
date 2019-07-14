using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    public class SegmentedControlButton : RadioButton
    {
        #region Konstruktory
        public SegmentedControlButton(Context context) : base(context) { }


        public SegmentedControlButton(Context context, IAttributeSet attrs) : base(context, attrs) { }


        public SegmentedControlButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }


        public SegmentedControlButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }


        protected SegmentedControlButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        #endregion
    }
}