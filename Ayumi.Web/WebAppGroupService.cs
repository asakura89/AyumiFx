using System;
using System.Collections.Generic;
using System.Linq;
using Arvy;
using AutoMapper;
using Databossy;
using WebApp.Model;
using WebApp.Utility;

namespace WebApp {
    public class WebAppGroupService : IGroupService {
        readonly IDbContextFactory factory;
        readonly IMapper mapper;

        public WebAppGroupService(IDbContextFactory factory) {
            this.factory = factory;
            mapper = new MapperConfiguration(conf => {
                    conf.CreateMap<m_Group, WebAppGroup>();
                    conf.CreateMap<WebAppGroup, m_Group>();
                })
                .CreateMapper();
        }

        public IList<WebAppGroup> GetAll(String tenant) {
            IList<m_Group> groups;
            using (IDatabase db = factory.Context)
                groups = db.Query<m_Group>(this.LoadEmbeddedEqx("GetAll")).ToList();

            return mapper.Map<IList<m_Group>, IList<WebAppGroup>>(groups);
        }

        public WebAppGroup Get(String tenant, String id) {
            m_Group group;
            using (IDatabase db = factory.Context)
                group = db.QuerySingle<m_Group>(this.LoadEmbeddedEqx("Get"), id);

            return mapper.Map<m_Group, WebAppGroup>(group);
        }

        public Boolean Find(String tenant, String id) {
            using (IDatabase db = factory.Context)
                return db.QueryScalar<Boolean>(this.LoadEmbeddedEqx("Find"), id);
        }

        public void Add(String tenant, WebAppGroup data) {
            m_Group group = mapper.Map<WebAppGroup, m_Group>(data);
            String joinedAuths = data
                .Users
                .SelectMany(user => data
                    .Activities
                    .Select(act => new m_Authorization {GroupId = group.GroupId, Username = user, ActivityId = act}))
                .AsDelimitedString(";", ",",
                    auth => auth.GroupId,
                    auth => auth.Username,
                    auth => auth.ActivityId);

            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Add"), group);
                result.AsActionResponseViewModel();

                result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("AddActivities"),
                    new {JoinedAuths = joinedAuths});
                result.AsActionResponseViewModel();
            }
        }

        public void Update(String tenant, String id, WebAppGroup data) {
            m_Group group = mapper.Map<WebAppGroup, m_Group>(data);
            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Update"), group);
                result.AsActionResponseViewModel();
            }
        }

        public void Remove(String tenant, String id) {
            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Remove"), new {Ids = id});
                result.AsActionResponseViewModel();
            }
        }
    }
}