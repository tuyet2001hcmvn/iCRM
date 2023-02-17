using ISD.EntityModels;
using ISD.Repositories.Report;
using ISD.Repositories.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class UnitOfWork
    {
        private EntityDataContext _context;

        private ProfileRepository _profileRepository;
        private TaskRepository _taskRepository;
        private TaskStatusRepository _taskStatusRepository;
        private StoreRepository _storeRepository;
        private WorkFlowRepository _workFlowRepository;
        private CatalogRepository _catalogRepository;
        private SalesEmployeeRepository _repoSaleEmployee;
        private AccountRepository _repoAccount;
        private AppointmentRepository _appointmentRepository;
        private UtilitiesRepository _utilitiesRepository;
        private KanbanTaskRepository _kanbanTaskRepository;
        private CommonDateRepository _commonDateRepository;
        private PersonInChargeRepository _personInChargeRepository;
        private RoleInChargeRepository _roleInChargeRepository;
        private ProfileGroupRepository _profileGroupRepository;
        private CompanyRepository _companyRepository;
        private StockRepository _stockRepository;
        private InventoryRepository _inventoryRepository;
        private DistrictRepository _districtRepository;
        private ProfileCareerRepository _profileCareerRepository;
        private ConstructionRepository _constructionRepository;
        private PivotGridTemplateRepository _pivotGridTemplateRepository;
        private RequestEccEmailConfigRepository _requestEccEmailConfigRepository;
        private SAPReportRepository _SAPReportRepository;
        private CampaignRepository _CampaignRepository;
        private ContentRepository _ContentRepository;
        private CertificateACRepository _certificateACRepository;
        private EmailTemplateConfigRepository _EmailTemplateConfigRepository;
        private EmployeeRatingsRepository _EmployeeRatingsRepository;
        private CostOfUsingCatalogueComparedToApprovalRepository _costOfUsingCatalogueComparedToApprovalRepository;
        private ProductPromotionRepository _productPromotionRepository;

        public UnitOfWork(EntityDataContext entities)
        {
            _context = entities;
        }

        public ProfileRepository ProfileRepository
        {
            get
            {
                if (_profileRepository == null)
                {
                    _profileRepository = new ProfileRepository(_context);
                }
                return _profileRepository;
            }
        }

        public EmployeeRatingsRepository EmployeeRatingsRepository
        {
            get
            {
                if (_EmployeeRatingsRepository == null)
                {
                    _EmployeeRatingsRepository = new EmployeeRatingsRepository(_context);
                }
                return _EmployeeRatingsRepository;
            }
        }
        public RequestEccEmailConfigRepository RequestEccEmailConfigRepository
        {
            get
            {
                if (_requestEccEmailConfigRepository == null)
                {
                    _requestEccEmailConfigRepository = new RequestEccEmailConfigRepository(_context);
                }
                return _requestEccEmailConfigRepository;
            }
        }
        public PivotGridTemplateRepository PivotGridTemplateRepository
        {
            get
            {
                if (_pivotGridTemplateRepository == null)
                {
                    _pivotGridTemplateRepository = new PivotGridTemplateRepository(_context);
                }
                return _pivotGridTemplateRepository;
            }
        }


        public TaskRepository TaskRepository
        {
            get
            {
                if (_taskRepository == null)
                {
                    _taskRepository = new TaskRepository(_context);
                }
                return _taskRepository;
            }
        }

        public TaskStatusRepository TaskStatusRepository
        {
            get
            {
                if (_taskStatusRepository == null)
                {
                    _taskStatusRepository = new TaskStatusRepository(_context);
                }
                return _taskStatusRepository;
            }
        }

        public StoreRepository StoreRepository
        {
            get
            {
                if (_storeRepository == null)
                {
                    _storeRepository = new StoreRepository(_context);
                }
                return _storeRepository;
            }
        }
        public WorkFlowRepository WorkFlowRepository
        {
            get
            {
                if (_workFlowRepository == null)
                {
                    _workFlowRepository = new WorkFlowRepository(_context);
                }
                return _workFlowRepository;
            }
        }
        public CatalogRepository CatalogRepository
        {
            get
            {
                if (_catalogRepository == null)
                {
                    _catalogRepository = new CatalogRepository(_context);
                }
                return _catalogRepository;
            }
        }
        public SalesEmployeeRepository SalesEmployeeRepository
        {
            get
            {
                if (_repoSaleEmployee == null)
                {
                    _repoSaleEmployee = new SalesEmployeeRepository(_context);
                }
                return _repoSaleEmployee;
            }
        }
        public AccountRepository AccountRepository
        {
            get
            {
                if (_repoAccount == null)
                {
                    _repoAccount = new AccountRepository(_context);
                }
                return _repoAccount;
            }
        }
        public AppointmentRepository AppointmentRepository
        {
            get
            {
                if (_appointmentRepository == null)
                {
                    _appointmentRepository = new AppointmentRepository(_context);
                }
                return _appointmentRepository;
            }
        }
        public ProductPromotionRepository ProductPromotionRepository
        {
            get
            {
                if (_productPromotionRepository == null)
                {
                    _productPromotionRepository = new ProductPromotionRepository(_context);
                }
                return _productPromotionRepository;
            }
        }
        public UtilitiesRepository UtilitiesRepository
        {
            get
            {
                if (_utilitiesRepository == null)
                {
                    _utilitiesRepository = new UtilitiesRepository();
                }
                return _utilitiesRepository;
            }
        }
        public KanbanTaskRepository KanbanTaskRepository
        {
            get
            {
                if (_kanbanTaskRepository == null)
                {
                    _kanbanTaskRepository = new KanbanTaskRepository(_context);
                }
                return _kanbanTaskRepository;
            }
        }
        public CommonDateRepository CommonDateRepository
        {
            get
            {
                if (_commonDateRepository == null)
                {
                    _commonDateRepository = new CommonDateRepository(_context);
                }
                return _commonDateRepository;
            }
        }
        public PersonInChargeRepository PersonInChargeRepository
        {
            get
            {
                if (_personInChargeRepository == null)
                {
                    _personInChargeRepository = new PersonInChargeRepository(_context);
                }
                return _personInChargeRepository;
            }
        }
        public RoleInChargeRepository RoleInChargeRepository
        {
            get
            {
                if (_roleInChargeRepository == null)
                {
                    _roleInChargeRepository = new RoleInChargeRepository(_context);
                }
                return _roleInChargeRepository;
            }
        }

        public ProfileGroupRepository ProfileGroupRepository
        {
            get
            {
                if (_profileGroupRepository == null)
                {
                    _profileGroupRepository = new ProfileGroupRepository(_context);
                }
                return _profileGroupRepository;
            }
        }
        public CompanyRepository CompanyRepository
        {
            get
            {
                if (_companyRepository == null)
                {
                    _companyRepository = new CompanyRepository(_context);
                }
                return _companyRepository;
            }
        }
        public StockRepository StockRepository
        {
            get
            {
                if (_stockRepository == null)
                {
                    _stockRepository = new StockRepository(_context);
                }
                return _stockRepository;
            }
        }
        #region Inventory

        private StockReceivingMasterRepository _stockReceivingMasterRepository;
        public StockReceivingMasterRepository StockReceivingMasterRepository
        {
            get
            {
                if (_stockReceivingMasterRepository == null)
                {
                    _stockReceivingMasterRepository = new StockReceivingMasterRepository(_context);
                }
                return _stockReceivingMasterRepository;
            }
        }

        private TransferRepository _transferRepository;
        public TransferRepository TransferRepository
        {
            get
            {
                if (_transferRepository == null)
                {
                    _transferRepository = new TransferRepository(_context);
                }
                return _transferRepository;
            }
        }
        private TransferDetailRepository _transferDetailRepository;
        public TransferDetailRepository TransferDetailRepository
        {
            get
            {
                if (_transferDetailRepository == null)
                {
                    _transferDetailRepository = new TransferDetailRepository(_context);
                }
                return _transferDetailRepository;
            }
        }

        public InventoryRepository InventoryRepository
        {
            get
            {
                if (_inventoryRepository == null)
                {
                    _inventoryRepository = new InventoryRepository(_context);
                }
                return _inventoryRepository;
            }
        }
        #endregion

        private SendSMSRepository _sendSMSRepository;
        public SendSMSRepository SendSMSRepository
        {
            get
            {
                if (_sendSMSRepository == null)
                {
                    _sendSMSRepository = new SendSMSRepository(_context);
                }
                return _sendSMSRepository;
            }
        }
        private RepositoryLibrary _repositoryLibrary;
        public RepositoryLibrary RepositoryLibrary
        {
            get
            {
                if (_repositoryLibrary == null)
                {
                    _repositoryLibrary = new RepositoryLibrary();
                }
                return _repositoryLibrary;
            }
        }

        private ProductRepository _productRepository;
        public ProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        private StockRecevingDetailRepository _stockRecevingDetailRepository;
        public StockRecevingDetailRepository StockRecevingDetailRepository
        {
            get
            {
                if (_stockRecevingDetailRepository == null)
                {
                    _stockRecevingDetailRepository = new StockRecevingDetailRepository(_context);
                }
                return _stockRecevingDetailRepository;
            }
        }
        private ProfileLevelRepository _profileLevelRepository;
        public ProfileLevelRepository ProfileLevelRepository
        {
            get
            {
                if (_profileLevelRepository == null)
                {
                    _profileLevelRepository = new ProfileLevelRepository(_context);
                }
                return _profileLevelRepository;
            }
        }

        private CatalogueRepository _catalogueRepository;
        public CatalogueRepository CatalogueRepository
        {
            get
            {
                if (_catalogueRepository == null)
                {
                    _catalogueRepository = new CatalogueRepository(_context);
                }
                return _catalogueRepository;
            }
        }


        private CustomerTasteRepository _customerTasteRepository;
        public CustomerTasteRepository CustomerTasteRepository
        {
            get
            {
                if (_customerTasteRepository == null)
                {
                    _customerTasteRepository = new CustomerTasteRepository(_context);
                }
                return _customerTasteRepository;
            }
        }
        private ProfileContactRepository _profileContactRepository;
        public ProfileContactRepository ProfileContactRepository
        {
            get
            {
                if (_profileContactRepository == null)
                {
                    _profileContactRepository = new ProfileContactRepository(_context);
                }
                return _profileContactRepository;
            }
        }
        private RevenueRepository _revenueRepository;
        public RevenueRepository RevenueRepository
        {
            get
            {
                if (_revenueRepository == null)
                {
                    _revenueRepository = new RevenueRepository(_context);
                }
                return _revenueRepository;
            }
        }
        private ProductWarrantyRepository _productWarrantyRepository;
        public ProductWarrantyRepository ProductWarrantyRepository
        {
            get
            {
                if (_productWarrantyRepository == null)
                {
                    _productWarrantyRepository = new ProductWarrantyRepository(_context);
                }
                return _productWarrantyRepository;
            }
        }
        private WarrantyRepository _warrantyRepository;
        public WarrantyRepository WarrantyRepository
        {
            get
            {
                if (_warrantyRepository == null)
                {
                    _warrantyRepository = new WarrantyRepository(_context);
                }
                return _warrantyRepository;
            }
        }
        private KanbanRepository _kanbanRepository;
        public KanbanRepository KanbanRepository
        {
            get
            {
                if (_kanbanRepository == null)
                {
                    _kanbanRepository = new KanbanRepository(_context);
                }
                return _kanbanRepository;
            }
        }
        private NewsRepository _newsRepository;
        public NewsRepository NewsRepository
        {
            get
            {
                if (_newsRepository == null)
                {
                    _newsRepository = new NewsRepository(_context);
                }
                return _newsRepository;
            }
        }
        private MobileKanbanRepository _mobileKanbanRepository;
        public MobileKanbanRepository MobileKanbanRepository
        {
            get
            {
                if (_mobileKanbanRepository == null)
                {
                    _mobileKanbanRepository = new MobileKanbanRepository(_context);
                }
                return _mobileKanbanRepository;
            }
        }

        private DeliveryRepository _deliveryRepository;
        public DeliveryRepository DeliveryRepository
        {
            get
            {
                if (_deliveryRepository == null)
                {
                    _deliveryRepository = new DeliveryRepository(_context);
                }
                return _deliveryRepository;
            }
        }
        private ConfigUtilities _configUtilities;
        public ConfigUtilities ConfigUtilities
        {
            get
            {
                if (_configUtilities == null)
                {
                    _configUtilities = new ConfigUtilities();
                }
                return _configUtilities;
            }
        }

        private ProvinceRepository _provinceRepository;
        public ProvinceRepository ProvinceRepository
        {
            get
            {
                if (_provinceRepository == null)
                {
                    _provinceRepository = new ProvinceRepository(_context);
                }
                return _provinceRepository;
            }
        }

        public DistrictRepository DistrictRepository
        {
            get
            {
                if (_districtRepository == null)
                {
                    _districtRepository = new DistrictRepository(_context);
                }
                return _districtRepository;
            }
        }

        private AddressBookRepository _addBookRepository;
        public AddressBookRepository AddressBookRepository
        {
            get
            {
                if (_addBookRepository == null)
                {
                    _addBookRepository = new AddressBookRepository(_context);
                }
                return _addBookRepository;
            }
        }

        public ProfileCareerRepository ProfileCareerRepository
        {
            get
            {
                if (_profileCareerRepository == null)
                {
                    _profileCareerRepository = new ProfileCareerRepository(_context);
                }
                return _profileCareerRepository;
            }
        }

        public ConstructionRepository ConstructionRepository
        {
            get
            {
                if (_constructionRepository == null)
                {
                    _constructionRepository = new ConstructionRepository(_context);
                }
                return _constructionRepository;
            }
        }

        public  SAPReportRepository SAPReportRepository
        {
            get
            {
                if (_SAPReportRepository == null)
                {
                    _SAPReportRepository = new SAPReportRepository(_context);
                }
                return _SAPReportRepository;
            }
        }

        public CertificateACRepository CertificateACRepository
        {
            get
            {
                if (_certificateACRepository == null)
                {
                    _certificateACRepository = new CertificateACRepository(_context);
                }
                return _certificateACRepository;
            }
        }

        public CostOfUsingCatalogueComparedToApprovalRepository CostOfUsingCatalogueComparedToApprovalRepository
        {
            get
            {
                if (_costOfUsingCatalogueComparedToApprovalRepository == null)
                {
                    _costOfUsingCatalogueComparedToApprovalRepository = new CostOfUsingCatalogueComparedToApprovalRepository(_context);
                }
                return _costOfUsingCatalogueComparedToApprovalRepository;
            }
        }

        public EmailTemplateConfigRepository EmailTemplateConfigRepository
        {
            get
            {
                if (_EmailTemplateConfigRepository == null)
                {
                    _EmailTemplateConfigRepository = new EmailTemplateConfigRepository(_context);
                }
                return _EmailTemplateConfigRepository;
            }
        }



        #region Marketing
        public CampaignRepository CampaignRepository
        {
            get
            {
                if (_CampaignRepository == null)
                {
                    _CampaignRepository = new CampaignRepository(_context);
                }
                return _CampaignRepository;
            }
        }
        public ContentRepository ContentRepository
        {
            get
            {
                if (_ContentRepository == null)
                {
                    _ContentRepository = new ContentRepository(_context);
                }
                return _ContentRepository;
            }
        }
        #endregion
    }
}
