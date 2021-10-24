using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Databossy;
using WebApp.Model;

namespace WebApp {
    public class WebAppMemberService : IGroupMemberService {
        readonly IDbContextFactory factory;
        readonly IMapper mapper;

        public WebAppMemberService(IDbContextFactory factory) {
            this.factory = factory;
            var config = new MapperConfiguration(conf => conf.CreateMap<m_Member, WebAppGroupMember>());
            mapper = config.CreateMapper();
        }

        public IList<WebAppGroupMember> GetByUser(String username) {
            using (IDatabase db = factory.Context)
                return db
                    .Query<m_Member>("SELECT * FROM m_Member WHERE Username = @0", username)
                    .Select(mapper.Map<m_Member, WebAppGroupMember>)
                    .ToList();
        }

        public IList<WebAppGroupMember> GetByGroup(String groupId) {
            using (IDatabase db = factory.Context)
                return db
                    .Query<m_Member>("SELECT * FROM m_Member WHERE GroupId = @0", groupId)
                    .Select(mapper.Map<m_Member, WebAppGroupMember>)
                    .ToList();
        }
    }
}