using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories.Infrastructure.Extensions
{
    public static class AddressBookExtension
    {
        public static void MapAddressBook(this AddressBookModel addressBookModel, AddressBookViewModel viewModel)
        {
            //1. GUID
            addressBookModel.AddressBookId = viewModel.AddressBookId;
            //2. Mã Profile
            addressBookModel.ProfileId = viewModel.ProfileId;
            //3. Loại địa chỉ
            addressBookModel.AddressTypeCode = viewModel.AddressTypeCode;
            //4. Địa chỉ
            addressBookModel.Address = viewModel.Address;
            //4. Địa chỉ 2
            addressBookModel.Address2 = viewModel.Address2;
            //5. Quận / Huyện
            addressBookModel.ProvinceId = viewModel.ProvinceId;
            //6. Tỉnh / Thành phố
            addressBookModel.DistrictId = viewModel.DistrictId;
            //7. Phường xã
            addressBookModel.WardId = viewModel.WardId;
            //8. Quốc Gia
            addressBookModel.CountryCode = viewModel.CountryCode;
            //9. Ghi chú
            addressBookModel.Note = viewModel.Note;
            //10. Địa chỉ chính
            addressBookModel.isMain = viewModel.isMain;
        }
    }
}
