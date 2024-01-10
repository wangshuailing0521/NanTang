using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace YJ.K3.NanTang.Pur.PlugIn
{
    [Description("采购订单 保存操作服务插件")]
    [HotUpdate]
    public class PoorderSave : AbstractOperationServicePlugIn
    {
        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);

            e.FieldKeys.Add("F_QAPZ_Amount");
            e.FieldKeys.Add("F_QAPZ_Amount2");
            e.FieldKeys.Add("FQty");
            e.FieldKeys.Add("FEntryAmount");
        }

        public override void EndOperationTransaction(EndOperationTransactionArgs e)
        {
            base.EndOperationTransaction(e);

            foreach (var billObj in e.DataEntitys)
            {
                RefreshEntryAmount(billObj);
            }
        }

        void RefreshEntryAmount(DynamicObject billObj)
        {
            DynamicObjectCollection entrys = billObj["POOrderEntry"] as DynamicObjectCollection;
            int counts = entrys.Count;
            decimal billAmount = Convert.ToDecimal(billObj["F_QAPZ_Amount"]);
            decimal kingdeeBillAmount = entrys.Sum(x => Convert.ToDecimal(x["Amount"]));
            decimal difAmount = billAmount - kingdeeBillAmount;

            int index = 1;

            foreach (var entry in entrys)
            {
                if (index == counts)
                {
                    decimal qty = Convert.ToDecimal(entry["Qty"]);
                    decimal amount = Convert.ToDecimal(entry["Amount"]);
                    amount = amount + difAmount;
                    entry["Amount"] = amount;
                    entry["Amount_LC"] = amount;
                    entry["AllAmount"] = amount;
                    entry["AllAmount_LC"] = amount;
                    entry["Price"] = amount / qty;
                    entry["TaxPrice"] = amount / qty;
                }

                index++;
            }
        }
    }
}
