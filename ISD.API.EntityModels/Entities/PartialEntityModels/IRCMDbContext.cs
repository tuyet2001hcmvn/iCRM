using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ISD.API.EntityModels.Entities
{
    public partial class ICRMDbContext : DbContext
    {
        public ICRMDbContext(Guid? currentAccountId)
        {
            CurrentAccountId = currentAccountId;
        }

        public Guid? CurrentAccountId { get; set; }

        /// <summary>
        /// Lấy user đang đăng nhập để lưu log
        /// </summary>
        /// <returns></returns>
        public Guid GetAccountId()
        {
            return Guid.Parse("d3d0cb44-0e76-40d0-8d90-d960dfbdd53a");
            //if (ClaimsPrincipal.Current.FindFirst(ClaimTypes.Sid) == null)
            //{
            //    return Guid.Parse("d3d0cb44-0e76-40d0-8d90-d960dfbdd53a");
            //}
            //string user = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Sid).Value;
            //return Guid.Parse(user);
        }

        /// <summary>
        /// Lấy value khóa chính của bảng để lưu log trace
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        object GetPrimaryKeyValue<TEntity>(TEntity entity)
        {

            var keyName = Model.FindEntityType(entity.GetType()).FindPrimaryKey().Properties
       .Select(x => x.Name).Single();
            var keyValue = entity.GetType().GetProperty(keyName).GetValue(entity, null);
            return keyValue;
        }

        /// <summary>
        /// Lưu log khi modified savechanges
        /// 1. Select những entry type là Modified (cập nhật)
        /// 2. Select các field cần log: tên bảng, khóa chính, data cũ, data mới, user sửa, thời gian sửa
        /// 3. So sánh data cũ != data mới -> Lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            //1.
            var modifiedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();

            foreach (var change in modifiedEntities)
            {
                //2.
                //Tên bảng
                Type entityType = change.Entity.GetType();
                if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                {
                    entityType = entityType.BaseType;
                }
                string entityName = entityType.Name;
                //Khóa chính
                var primaryKey = GetPrimaryKeyValue(change.Entity);
                var props = change.Metadata.GetProperties();
                foreach (var prop in props)
                {

                    if (prop.Name != "CreateBy" && prop.Name != "CreateTime" && prop.Name != "LastEditTime" && prop.Name != "LastEditBy")
                    {
                        //Convert data cũ và data mới thành cùng kiểu dữ liệu gốc để so sánh
                        //VD: kiểu dữ liệu gốc là decimal -> Convert string thành decimal
                        //Data cũ
                        object originalValue = null;
                        if (change.OriginalValues[prop.Name] != null)
                        {
                            var type = change.OriginalValues[prop.Name].GetType();
                            if (type == typeof(Guid))
                            {
                                originalValue = Guid.Parse(change.OriginalValues[prop.Name].ToString());
                            }
                            else
                            {
                                originalValue = Convert.ChangeType(change.OriginalValues[prop.Name].ToString(), change.OriginalValues[prop.Name].GetType());
                            }
                        }

                        //Data mới
                        object currentValue = null;
                        if (change.CurrentValues[prop.Name] != null)
                        {
                            var type = change.CurrentValues[prop.Name].GetType();
                            if (type == typeof(Guid))
                            {
                                currentValue = Guid.Parse(change.CurrentValues[prop.Name].ToString());
                            }
                            else
                            {
                                currentValue = Convert.ChangeType(change.CurrentValues[prop.Name].ToString(), change.CurrentValues[prop.Name].GetType());
                            }
                        }

                        //3.
                        if (!object.Equals(originalValue ?? "", currentValue ?? ""))
                        {
                            ChangeDataLogModel log = new ChangeDataLogModel()
                            {
                                LogId = Guid.NewGuid(),
                                PrimaryKey = Guid.Parse(primaryKey.ToString()),
                                TableName = entityName,
                                FieldName = prop.Name,
                                OldData = string.Format("{0}", originalValue),
                                NewData = string.Format("{0}", currentValue),
                                LastEditBy = CurrentAccountId != null ? CurrentAccountId : GetAccountId(),
                                LastEditTime = DateTime.Now
                            };
                            Add(log);
                        }
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}
