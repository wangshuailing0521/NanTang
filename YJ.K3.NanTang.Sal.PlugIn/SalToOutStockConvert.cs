using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn;
using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn.Args;
using Kingdee.BOS.Core.Metadata.FieldElement;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;

using System;
using System.ComponentModel;
using System.Linq;

namespace YJ.K3.NanTang.Sal.PlugIn
{
    [Description("销售订单-销售出库单单据转换插件")]
    [HotUpdate]
    public class SalToOutStockConvert: AbstractConvertPlugIn
    {
        public override void OnAfterFieldMapping(AfterFieldMappingEventArgs e)
        {
            base.OnAfterFieldMapping(e);

            ExtendedDataEntity[] heads = e.TargetExtendDataEntitySet.FindByEntityKey("FBillHead");
            if (heads == null)
            {
                return;
            }
            foreach (ExtendedDataEntity head in heads)
            {
                AddSubEntry(e, head);
            }
        }

        void AddSubEntry(AfterFieldMappingEventArgs e, ExtendedDataEntity head)
        {
            DynamicObject billObj = head.DataEntity;

            DynamicObjectCollection entrys = billObj["SAL_OUTSTOCKENTRY"] as DynamicObjectCollection;
            foreach (var entry in entrys)
            {
                long sourceEntryID = Convert.ToInt64(entry["SOEntryId"]);


                AddYMXSubEntity(entry, sourceEntryID);
                AddLazadaSubEntity(entry, sourceEntryID);
                AddSMTSubEntity(entry, sourceEntryID);
                AddShopeeSubEntity(entry, sourceEntryID);
                AddEbaySubEntity(entry, sourceEntryID);
                AddWEMSubEntity(entry, sourceEntryID);

            }
        }

        #region 亚马逊

        /// <summary>
        /// 获取源单亚马逊信息
        /// </summary>
        /// <param name="srcBillNo"></param>
        /// <returns></returns>
        DynamicObjectCollection GetSrcYMXEntry(long sourceEntryID)
        {
            string sql = $@"
                SELECT  *
                  FROM  QAPZ_t_Cust_Entry100006 A WITH(NOLOCK)
                 WHERE  FEntryID = {sourceEntryID}";
            return DBUtils.ExecuteDynamicObject(this.Context, sql);
        }

        /// <summary>
        /// 新增亚马逊明细
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceEntryID"></param>
        void AddYMXSubEntity(DynamicObject entry, long sourceEntryID)
        {
            DynamicObjectCollection srcRows = GetSrcYMXEntry(sourceEntryID);
            DynamicObjectCollection subEntrys = entry["QAPZ_Cust_Entry100007"] as DynamicObjectCollection;
            subEntrys.Clear();
            int index = 1;
            foreach (var srcRow in srcRows)
            {
                DynamicObject newRow = new DynamicObject(subEntrys.DynamicCollectionItemPropertyType);
                newRow["F_QAPZ_Amount"] = srcRow["F_QAPZ_Amount"];
                newRow["F_QAPZ_Amount1"] = srcRow["F_QAPZ_Amount7"];
                newRow["F_QAPZ_Amount2"] = srcRow["F_QAPZ_Amount8"];
                newRow["F_QAPZ_Amount3"] = srcRow["F_QAPZ_Amount9"];
                newRow["F_QAPZ_Amount4"] = srcRow["F_QAPZ_Amount10"];
                newRow["F_QAPZ_Amount5"] = srcRow["F_QAPZ_Amount11"];
                newRow["F_QAPZ_Amount6"] = srcRow["F_QAPZ_Amount12"];
                newRow["F_QAPZ_Amount7"] = srcRow["F_QAPZ_Amount17"];
                newRow["F_QAPZ_Amount8"] = srcRow["F_QAPZ_Amount18"];
                newRow["F_QAPZ_Amount9"] = srcRow["F_QAPZ_Amount19"];
                newRow["F_QAPZ_Amount27"] = srcRow["F_QAPZ_Amount27"];

                subEntrys.Add(newRow);
                index++;
            }
        }

        #endregion

        #region lazada

        /// <summary>
        /// 获取源单Lazada信息
        /// </summary>
        /// <param name="srcBillNo"></param>
        /// <returns></returns>
        DynamicObjectCollection GetSrcLazadaEntry(long sourceEntryID)
        {
            string sql = $@"
                SELECT  *
                  FROM  QAPZ_t_Cust_Entry100005 A WITH(NOLOCK)
                 WHERE  FEntryID = {sourceEntryID}";
            return DBUtils.ExecuteDynamicObject(this.Context, sql);
        }

        /// <summary>
        /// 新增Lazada明细
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceEntryID"></param>
        void AddLazadaSubEntity(DynamicObject entry, long sourceEntryID)
        {
            DynamicObjectCollection srcRows = GetSrcLazadaEntry(sourceEntryID);
            DynamicObjectCollection subEntrys = entry["QAPZ_Cust_Entry100008"] as DynamicObjectCollection;
            subEntrys.Clear();
            int index = 1;
            foreach (var srcRow in srcRows)
            {
                DynamicObject newRow = new DynamicObject(subEntrys.DynamicCollectionItemPropertyType);
                newRow["F_QAPZ_Amount10"] = srcRow["F_QAPZ_Amount13"];
                newRow["F_QAPZ_Amount11"] = srcRow["F_QAPZ_Amount14"];
                newRow["F_QAPZ_Amount12"] = srcRow["F_QAPZ_Amount15"];
                newRow["F_QAPZ_Amount13"] = srcRow["F_QAPZ_Amount16"];
                newRow["F_QAPZ_Amount33"] = srcRow["F_QAPZ_Amount34"];
                newRow["F_QAPZ_Amount28"] = srcRow["F_QAPZ_Amount32"];

                subEntrys.Add(newRow);
                index++;
            }
        }

        #endregion

        #region 速卖通

        /// <summary>
        /// 获取源单速卖通信息
        /// </summary>
        /// <param name="srcBillNo"></param>
        /// <returns></returns>
        DynamicObjectCollection GetSrcSMTEntry(long sourceEntryID)
        {
            string sql = $@"
                SELECT  *
                  FROM  QAPZ_t_Cust_Entry100011 A WITH(NOLOCK)
                 WHERE  FEntryID = {sourceEntryID}";
            return DBUtils.ExecuteDynamicObject(this.Context, sql);
        }

        /// <summary>
        /// 新增速卖通明细
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceEntryID"></param>
        void AddSMTSubEntity(DynamicObject entry, long sourceEntryID)
        {
            DynamicObjectCollection srcRows = GetSrcSMTEntry(sourceEntryID);
            DynamicObjectCollection subEntrys = entry["QAPZ_Cust_Entry100015"] as DynamicObjectCollection;
            subEntrys.Clear();
            int index = 1;
            foreach (var srcRow in srcRows)
            {
                DynamicObject newRow = new DynamicObject(subEntrys.DynamicCollectionItemPropertyType);
                newRow["F_QAPZ_Amount14"] = srcRow["F_QAPZ_Amount1"];
                newRow["F_QAPZ_Amount15"] = srcRow["F_QAPZ_Amount2"];
                newRow["F_QAPZ_Amount16"] = srcRow["F_QAPZ_Amount3"];
                newRow["F_QAPZ_Amount17"] = srcRow["F_QAPZ_Amount4"];
                //newRow["F_QAPZ_Amount29"] = srcRow["F_QAPZ_Amount35"];
                newRow["F_QAPZ_Amount29"] = srcRow["F_QAPZ_Amount28"];

                subEntrys.Add(newRow);
                index++;
            }
        }

        #endregion

        #region Shopee

        /// <summary>
        /// 获取源单Shopee信息
        /// </summary>
        /// <param name="srcBillNo"></param>
        /// <returns></returns>
        DynamicObjectCollection GetSrcShopeeEntry(long sourceEntryID)
        {
            string sql = $@"
                SELECT  *
                  FROM  QAPZ_t_Cust_Entry100012 A WITH(NOLOCK)
                 WHERE  FEntryID = {sourceEntryID}";
            return DBUtils.ExecuteDynamicObject(this.Context, sql);
        }

        /// <summary>
        /// 新增Shopee明细
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceEntryID"></param>
        void AddShopeeSubEntity(DynamicObject entry, long sourceEntryID)
        {
            DynamicObjectCollection srcRows = GetSrcShopeeEntry(sourceEntryID);
            DynamicObjectCollection subEntrys = entry["QAPZ_Cust_Entry100016"] as DynamicObjectCollection;
            subEntrys.Clear();
            int index = 1;
            foreach (var srcRow in srcRows)
            {
                DynamicObject newRow = new DynamicObject(subEntrys.DynamicCollectionItemPropertyType);
                newRow["F_QAPZ_Amount18"] = srcRow["F_QAPZ_Amount5"];
                newRow["F_QAPZ_Amount19"] = srcRow["F_QAPZ_Amount6"];
                newRow["F_QAPZ_Amount34"] = srcRow["F_QAPZ_Amount33"];
                newRow["F_QAPZ_Amount30"] = srcRow["F_QAPZ_Amount29"];


                subEntrys.Add(newRow);
                index++;
            }
        }

        #endregion

        #region Ebay

        /// <summary>
        /// 获取源单Ebay信息
        /// </summary>
        /// <param name="srcBillNo"></param>
        /// <returns></returns>
        DynamicObjectCollection GetSrcEbayEntry(long sourceEntryID)
        {
            string sql = $@"
                SELECT  *
                  FROM  QAPZ_t_Cust_Entry100013 A WITH(NOLOCK)
                 WHERE  FEntryID = {sourceEntryID}";
            return DBUtils.ExecuteDynamicObject(this.Context, sql);
        }

        /// <summary>
        /// 新增Ebay明细
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceEntryID"></param>
        void AddEbaySubEntity(DynamicObject entry, long sourceEntryID)
        {
            DynamicObjectCollection srcRows = GetSrcEbayEntry(sourceEntryID);
            DynamicObjectCollection subEntrys = entry["QAPZ_Cust_Entry100017"] as DynamicObjectCollection;
            subEntrys.Clear();
            int index = 1;
            foreach (var srcRow in srcRows)
            {
                DynamicObject newRow = new DynamicObject(subEntrys.DynamicCollectionItemPropertyType);
                newRow["F_QAPZ_Amount20"] = srcRow["F_QAPZ_Amount21"];
                newRow["F_QAPZ_Amount21"] = srcRow["F_QAPZ_Amount22"];
                newRow["F_QAPZ_Amount31"] = srcRow["F_QAPZ_Amount30"];


                subEntrys.Add(newRow);
                index++;
            }
        }

        #endregion

        #region 沃尔玛

        /// <summary>
        /// 获取源单沃尔玛信息
        /// </summary>
        /// <param name="srcBillNo"></param>
        /// <returns></returns>
        DynamicObjectCollection GetSrcWEMntry(long sourceEntryID)
        {
            string sql = $@"
                SELECT  *
                  FROM  QAPZ_t_Cust_Entry100014 A WITH(NOLOCK)
                 WHERE  FEntryID = {sourceEntryID}";
            return DBUtils.ExecuteDynamicObject(this.Context, sql);
        }

        /// <summary>
        /// 新增沃尔玛y明细
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceEntryID"></param>
        void AddWEMSubEntity(DynamicObject entry, long sourceEntryID)
        {
            DynamicObjectCollection srcRows = GetSrcWEMntry(sourceEntryID);
            DynamicObjectCollection subEntrys = entry["QAPZ_Cust_Entry100018"] as DynamicObjectCollection;
            subEntrys.Clear();
            int index = 1;
            foreach (var srcRow in srcRows)
            {
                DynamicObject newRow = new DynamicObject(subEntrys.DynamicCollectionItemPropertyType);
                newRow["F_QAPZ_Amount22"] = srcRow["F_QAPZ_Amount23"];
                newRow["F_QAPZ_Amount23"] = srcRow["F_QAPZ_Amount24"];
                newRow["F_QAPZ_Amount24"] = srcRow["F_QAPZ_Amount25"];
                newRow["F_QAPZ_Amount25"] = srcRow["F_QAPZ_Amount26"];
                newRow["F_QAPZ_Amount32"] = srcRow["F_QAPZ_Amount31"];


                subEntrys.Add(newRow);
                index++;
            }
        }

        #endregion
    }
}
