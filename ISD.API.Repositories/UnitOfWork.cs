using ISD.API.EntityModels.Entities;
using ISD.API.Repositories.Marketing;
using ISD.API.Repositories.MasterData;
using ISD.API.Repositories.Permission;
using ISD.API.Repositories.Utilities;
using System;

namespace ISD.API.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private ICRMDbContext _context;
        private TargetGroupRepository targetGroupRepository;
        private MemberOfTargetGroupRepository memberOfTargetGroupRepository;
        private ProfileRepository profileRepository;
        private ContentRepository contentRepository;
        private StoreRepository storeRepository;
        private EmailAccountRepository emailAccountRepository;
        private CampaignRepository campaignRepository;
        private CatalogRepository catalogRepository;
        private SendMailCalendarRepository sendMailCalendarRepository;
        private UnfollowRepository unsubscribeRepository;
        private QuestionBankRepository questionBankRepository;
        private FavoriteReportRepository favoriteReportRepository;
        private SearchTemplateRepository searchTemplateRepository;
        private StockTransferRequestRepository stockTransferRequestRepository;
        private StockTransferRequestDetailRepository stockTransferRequestDetailRepository;
        private TransferDetailRepository transferDetailRepository;
        private TransferRepository transferRepository;
        private StockRepository stockRepository;
        private MemberOfExternalProfileTargetGroupRepository memberOfExternalProfileTargetGroupRepository;
        private TemplateAndGiftTargetGroupRepository templateAndGiftTargetGroupRepository;
        private TemplateAndGiftMemberRepository templateAndGiftMemberRepository;
        private TemplateAndGiftCampaignRepository templateAndGiftCampaignRepository;
        private TemplateAndGiftMemberAddressRepository templateAndGiftMemberAddressRepository;
        private RequestEccEmailConfigRepository requestEccEmailConfigRepository;
        private AccountRepository accountRepository;

        public UnitOfWork(ICRMDbContext context)
        {
            _context = context;
        }

        public RequestEccEmailConfigRepository RequestEccEmailConfigRepository
        {
            get
            {

                if (this.requestEccEmailConfigRepository == null)
                {
                    this.requestEccEmailConfigRepository = new RequestEccEmailConfigRepository(_context);
                }
                return requestEccEmailConfigRepository;
            }
        }
        public TemplateAndGiftMemberAddressRepository TemplateAndGiftMemberAddressRepository
        {
            get
            {

                if (this.templateAndGiftMemberAddressRepository == null)
                {
                    this.templateAndGiftMemberAddressRepository = new TemplateAndGiftMemberAddressRepository(_context);
                }
                return templateAndGiftMemberAddressRepository;
            }
        }
        public TemplateAndGiftCampaignRepository TemplateAndGiftCampaignRepository
        {
            get
            {

                if (this.templateAndGiftCampaignRepository == null)
                {
                    this.templateAndGiftCampaignRepository = new TemplateAndGiftCampaignRepository(_context);
                }
                return templateAndGiftCampaignRepository;
            }
        }
        public TemplateAndGiftMemberRepository TemplateAndGiftMemberRepository
        {
            get
            {

                if (this.templateAndGiftMemberRepository == null)
                {
                    this.templateAndGiftMemberRepository = new TemplateAndGiftMemberRepository(_context);
                }
                return templateAndGiftMemberRepository;
            }
        }
        public TemplateAndGiftTargetGroupRepository TemplateAndGiftTargetGroupRepository
        {
            get
            {

                if (this.templateAndGiftTargetGroupRepository == null)
                {
                    this.templateAndGiftTargetGroupRepository = new TemplateAndGiftTargetGroupRepository(_context);
                }
                return templateAndGiftTargetGroupRepository;
            }
        }

        public MemberOfExternalProfileTargetGroupRepository MemberOfExternalProfileTargetGroupRepository
        {
            get
            {

                if (this.memberOfExternalProfileTargetGroupRepository == null)
                {
                    this.memberOfExternalProfileTargetGroupRepository = new MemberOfExternalProfileTargetGroupRepository(_context);
                }
                return memberOfExternalProfileTargetGroupRepository;
            }
        }

        public TransferRepository TransferRepository
        {
            get
            {

                if (this.transferRepository == null)
                {
                    this.transferRepository = new TransferRepository(_context);
                }
                return transferRepository;
            }
        }
        public TransferDetailRepository TransferDetailRepository
        {
            get
            {

                if (this.transferDetailRepository == null)
                {
                    this.transferDetailRepository = new TransferDetailRepository(_context);
                }
                return transferDetailRepository;
            }
        }
        public StockRepository StockRepository
        {
            get
            {

                if (this.stockRepository == null)
                {
                    this.stockRepository = new StockRepository(_context);
                }
                return stockRepository;
            }
        }

        public StockTransferRequestDetailRepository StockTransferRequestDetailRepository
        {
            get
            {

                if (this.stockTransferRequestDetailRepository == null)
                {
                    this.stockTransferRequestDetailRepository = new StockTransferRequestDetailRepository(_context);
                }
                return stockTransferRequestDetailRepository;
            }
        }
        public StockTransferRequestRepository StockTransferRequestRepository
        {
            get
            {

                if (this.stockTransferRequestRepository == null)
                {
                    this.stockTransferRequestRepository = new StockTransferRequestRepository(_context);
                }
                return stockTransferRequestRepository;
            }
        }
        public FavoriteReportRepository FavoriteReportRepository
        {
            get
            {

                if (this.favoriteReportRepository == null)
                {
                    this.favoriteReportRepository = new FavoriteReportRepository(_context);
                }
                return favoriteReportRepository;
            }
        }

        public QuestionBankRepository QuestionBankRepository
        {
            get
            {

                if (this.questionBankRepository == null)
                {
                    this.questionBankRepository = new QuestionBankRepository(_context);
                }
                return questionBankRepository;
            }
        }
        public AccountRepository AccountRepository
        {
            get
            {

                if (this.accountRepository == null)
                {
                    this.accountRepository = new AccountRepository(_context);
                }
                return accountRepository;
            }
        }
        public UnfollowRepository UnfollowRepository
        {
            get
            {

                if (this.unsubscribeRepository == null)
                {
                    this.unsubscribeRepository = new UnfollowRepository(_context);
                }
                return unsubscribeRepository;
            }
        }
        public SendMailCalendarRepository SendMailCalendarRepository
        {
            get
            {

                if (this.sendMailCalendarRepository == null)
                {
                    this.sendMailCalendarRepository = new SendMailCalendarRepository(_context);
                }
                return sendMailCalendarRepository;
            }
        }
        public TargetGroupRepository TargetGroupRepository
        {
            get
            {

                if (this.targetGroupRepository == null)
                {
                    this.targetGroupRepository = new TargetGroupRepository(_context);
                }
                return targetGroupRepository;
            }
        }
        public CatalogRepository CatalogRepository
        {
            get
            {

                if (this.catalogRepository == null)
                {
                    this.catalogRepository = new CatalogRepository(_context);
                }
                return catalogRepository;
            }
        }
        public MemberOfTargetGroupRepository MemberOfTargetGroupRepository
        {
            get
            {

                if (this.memberOfTargetGroupRepository == null)
                {
                    this.memberOfTargetGroupRepository = new MemberOfTargetGroupRepository(_context);
                }
                return memberOfTargetGroupRepository;
            }
        }
        public ProfileRepository ProfileRepository
        {
            get
            {
                if (this.profileRepository == null)
                {
                    this.profileRepository = new ProfileRepository(_context);
                }
                return profileRepository;
            }
        }
        public ContentRepository ContentRepository
        {
            get
            {
                if (this.contentRepository == null)
                {
                    this.contentRepository = new ContentRepository(_context);
                }
                return contentRepository;
            }
        }
        public StoreRepository StoreRepository
        {
            get
            {
                if (this.storeRepository == null)
                {
                    this.storeRepository = new StoreRepository(_context);
                }
                return storeRepository;
            }
        }
        public EmailAccountRepository EmailAccountRepository
        {
            get
            {
                if (this.emailAccountRepository == null)
                {
                    this.emailAccountRepository = new EmailAccountRepository(_context);
                }
                return emailAccountRepository;
            }
        }
        public CampaignRepository CampaignRepository
        {
            get
            {
                if (this.campaignRepository == null)
                {
                    this.campaignRepository = new CampaignRepository(_context);
                }
                return campaignRepository;
            }
        }
        public SearchTemplateRepository SearchTemplateRepository
        {
            get
            {
                if (this.searchTemplateRepository == null)
                {
                    this.searchTemplateRepository = new SearchTemplateRepository(_context);
                }
                return searchTemplateRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
