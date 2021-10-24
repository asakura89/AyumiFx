using System;
using System.Collections.Generic;
using System.Linq;
using Arvy;
using AutoMapper;
using Databossy;
using WebApp.Config;
using WebApp.Model;
using WebApp.Utility;

namespace WebApp {
    public class WebAppUserService : IUserService {
        readonly IDbContextFactory factory;
        readonly IMapper mapper;

        public WebAppUserService(IDbContextFactory factory) {
            this.factory = factory;
            var config = new MapperConfiguration(conf => conf.CreateMap<m_User, WebAppUser>());
            mapper = config.CreateMapper();
        }

        public void UpdateBadSignInCounter(String username) {
            using (IDatabase db = factory.Context) {
                m_User user = GetByUsernamePure(username);
                user.Counter += 1;

                db.NExecute(this.LoadEmbeddedEqx("UpdateBadSignInCounter"), user);
            }
        }

        public IList<WebAppUser> GetAll(String tenant) {
            IList<WebAppUser> users;
            using (IDatabase db = factory.Context) {
                users = db.Query<WebAppUser>(this.LoadEmbeddedEqx("GetAll")).ToList();
            }

            if (users == null || users.Count == 0)
                throw new InvalidOperationException($"{nameof(users)} have invalid value.");

            return users;
        }

        public IList<WebAppUser> GetAllNonAdmin(String tenant, String adminGroupName) {
            IList<WebAppUser> users;
            using (IDatabase db = factory.Context) {
                users = db.Query<WebAppUser>(this.LoadEmbeddedEqx("GetAllNonAdmin"), adminGroupName).ToList();
            }

            if (users == null || users.Count == 0)
                throw new InvalidOperationException($"{nameof(users)} have invalid value.");

            return users;
        }

        public WebAppUser Get(String tenant, String id) {
            m_User user = GetByUsernamePure(id);

            return mapper.Map<m_User, WebAppUser>(user);
        }

        public Boolean Find(String tenant, String id) {
            using (IDatabase db = factory.Context)
                return db.QueryScalar<Boolean>(this.LoadEmbeddedEqx("Find"), id);
        }

        public void Add(String tenant, WebAppUser data) {
            data.Password = AccountExt.PasswordEnx(data.Username + MainConfig.NewUserInitialPasswordDnx);

            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Add"), data);
                result.AsActionResponseViewModel();
            }
        }

        public void Update(String tenant, String id, WebAppUser data) {
            m_User user = mapper.Map<WebAppUser, m_User>(data);
            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Update"), user);
                result.AsActionResponseViewModel();
            }
        }

        public void Remove(String tenant, String id) {
            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Remove"), new {Ids = id});
                result.AsActionResponseViewModel();
            }
        }

        m_User GetByUsernamePure(String username) {
            using (IDatabase db = factory.Context)
                return db.QuerySingle<m_User>(this.LoadEmbeddedEqx("Get"), username);
        }
    }
}