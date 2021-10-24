using System;
using System.Collections.Generic;

namespace WebApp {
    public interface IGroupMemberService {
        IList<WebAppGroupMember> GetByUser(String username);
        IList<WebAppGroupMember> GetByGroup(String groupId);
    }
}