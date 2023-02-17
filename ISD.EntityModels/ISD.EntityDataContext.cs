using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;

namespace ISD.EntityModels
{
    public partial class EntityDataContext : DbContext
    {
        /// <summary>
        /// LastModifiedUser
        /// </summary>
        /// <param name="currentAccountId"></param>
        public EntityDataContext(Guid? currentAccountId)
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
            if (ClaimsPrincipal.Current.FindFirst(ClaimTypes.Sid) == null)
            {
                return Guid.Parse("FD68F5F8-01F9-480F-ACB7-BA5D74D299C8");
            }
            string user = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Sid).Value;
            return Guid.Parse(user);
        }

        /// <summary>
        /// Lấy value khóa chính của bảng để lưu log trace
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
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
                var primaryKey = GetPrimaryKeyValue(change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    if (prop != "LastModifiedTime" && prop != "LastModifiedUser" && prop != "LastEditTime" && prop != "LastEditBy")
                    {
                        //Convert data cũ và data mới thành cùng kiểu dữ liệu gốc để so sánh
                        //VD: kiểu dữ liệu gốc là decimal -> Convert string thành decimal
                        //Data cũ
                        object originalValue = null;
                        if (change.OriginalValues[prop] != null)
                        {
                            var type = change.OriginalValues[prop].GetType();
                            if (type == typeof(Guid))
                            {
                                originalValue = Guid.Parse(change.OriginalValues[prop].ToString());
                            }
                            else
                            {
                                originalValue = Convert.ChangeType(change.OriginalValues[prop].ToString(), change.OriginalValues[prop].GetType());
                            }
                        }

                        //Data mới
                        object currentValue = null;
                        if (change.CurrentValues[prop] != null)
                        {
                            var type = change.CurrentValues[prop].GetType();
                            if (type == typeof(Guid))
                            {
                                currentValue = Guid.Parse(change.CurrentValues[prop].ToString());
                            }
                            else
                            {
                                currentValue = Convert.ChangeType(change.CurrentValues[prop].ToString(), change.CurrentValues[prop].GetType());
                            }
                        }

                        //3.
                        if (!object.Equals(originalValue??"", currentValue??""))
                        {
                            if(entityType.Name== "SalesEmployeeModel")
                            {
                                ChangeDataLogModel log = new ChangeDataLogModel()
                                {
                                    LogId = Guid.NewGuid(),
                                   // PrimaryKey = primaryKey.ToString(),
                                    TableName = entityName,
                                    FieldName = prop,
                                    OldData = string.Format("{0}", originalValue),
                                    NewData = string.Format("{0}", currentValue),
                                    LastEditBy = CurrentAccountId != null ? CurrentAccountId : GetAccountId(),
                                    LastEditTime = DateTime.Now
                                };
                                ChangeDataLogModel.Add(log);
                            }
                            else
                            {
                                ChangeDataLogModel log = new ChangeDataLogModel()
                                {
                                    LogId = Guid.NewGuid(),
                                    PrimaryKey = Guid.Parse(primaryKey.ToString()),
                                    TableName = entityName,
                                    FieldName = prop,
                                    OldData = string.Format("{0}", originalValue),
                                    NewData = string.Format("{0}", currentValue),
                                    LastEditBy = CurrentAccountId != null ? CurrentAccountId : GetAccountId(),
                                    LastEditTime = DateTime.Now
                                };
                                ChangeDataLogModel.Add(log);
                            }                      
                        }
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}
