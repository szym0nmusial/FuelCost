using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    public class MyItemTouchHelper : ItemTouchHelper.Callback
    {

        Context Context;
        public RecyclerViewAdapter mAdapter;

        public MyItemTouchHelper(Context context, RecyclerViewAdapter adapter)
        {
            Context = context;
            mAdapter = adapter;
            //  mAdapter = recyclerViewAdapter;
            // you can pass any thing in your contractor , may be your RecyclerView adapter 
        }
        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            //int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            int swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;

            if (viewHolder.AdapterPosition == 10)
            {
                return 0;
            }
            return MakeMovementFlags(0, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            RecyclerViewViewHolder recyclerViewViewHolder = viewHolder as RecyclerViewViewHolder;
            recyclerViewViewHolder.Name.Text = "Swiped..";
            return false;
        }


        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            int position = viewHolder.AdapterPosition;
            //LocalSet.DelVehicle(LocalSet.VehicleDataList[position]);
            LocalSet.DelVehicle(position);
            mAdapter.NotifyItemRemoved(position);
            mAdapter.NotifyItemRangeChanged(position, mAdapter.ItemCount);

        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            var itemView = viewHolder.ItemView;
            var itemHeight = itemView.Bottom - itemView.Top;
            var isCanceled = dX == 0f && !isCurrentlyActive;

            if (isCanceled)
            {
                clearCanvas(c, itemView.Right + dX, (float)itemView.Top, (float)itemView.Right, (float)itemView.Bottom);
                base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                return;
            }

            // Draw the red delete background

            var background = new ColorDrawable();

            background.Color = Color.ParseColor("#f44336");
            background.SetBounds(itemView.Right + (int)dX, itemView.Top, itemView.Right, itemView.Bottom);
            background.Draw(c);

            var deleteIcon = ContextCompat.GetDrawable(Context, Resource.Drawable.del);
            var intrinsicHeight = deleteIcon.IntrinsicHeight;
            var intrinsicWidth = deleteIcon.IntrinsicWidth;

            // Calculate position of delete icon
            var deleteIconTop = itemView.Top + (itemHeight - intrinsicHeight) / 2;
            var deleteIconMargin = (itemHeight - intrinsicHeight) / 2;
            var deleteIconLeft = itemView.Right - deleteIconMargin - intrinsicWidth;
            var deleteIconRight = itemView.Right - deleteIconMargin;
            var deleteIconBottom = deleteIconTop + intrinsicHeight;

            // Draw the delete icon
            deleteIcon.SetBounds(deleteIconLeft, deleteIconTop, deleteIconRight, deleteIconBottom);
            deleteIcon.Draw(c);

            base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        }

        private void clearCanvas(Canvas c, float left, float top, float right, float bottom)
        {

            Paint clearPaint = new Paint();
            clearPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
            c?.DrawRect(left, top, right, bottom, clearPaint);
        }

    }
}