using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata.EntityElement;
using Kingdee.BOS.Core.Metadata.FieldElement;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace YJ.K3.NanTang.Pur.PlugIn
{
    [Description("采购订单维护插件")]
    [HotUpdate]
    public class PoorderEdit : AbstractBillPlugIn
    {
        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);

            List<string> files = new List<string>()
            {
                "F_QAPZ_Amount","F_QAPZ_Amount2","FQty","FPrice"
            };

            if (files.Contains(e.Field.Key))
            {
                RefreshEntryAmount();
            }
        }

        void RefreshEntryAmount()
        {
            DynamicObject billObj = this.Model.DataObject;
            DynamicObjectCollection entrys = billObj["POOrderEntry"] as DynamicObjectCollection;
            int counts = entrys.Count;
            decimal billAmount = Convert.ToDecimal(billObj["F_QAPZ_Amount"]);
            decimal kingdeeBillAmount = entrys.Sum(x => Convert.ToDecimal(x["AllAmount"]));
            decimal difAmount = billAmount - kingdeeBillAmount ;
            decimal amount = Convert.ToDecimal(this.View.Model.GetValue("FAllAmount", counts - 1));
            amount = amount + difAmount;
            this.View.Model.SetValue("FAllAmount", amount, counts - 1);
            this.View.InvokeFieldUpdateService("FAllAmount", counts - 1);
            this.View.UpdateView("FAllAmount", counts - 1);
        }
    }
}
