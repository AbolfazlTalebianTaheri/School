using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace School.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}")]
    public class BaseController : ControllerBase
    {
        /// <summary>کاربر والد است؟ (والدین فقط دانش‌آموزان وصل‌شده به حساب خودشان را می‌بینند)</summary>
        protected bool IsParent => User.IsInRole("Parent");

        /// <summary>شناسه‌ی دانش‌آموزان حساب والد (از Claim های student_id توکن)</summary>
        protected HashSet<ushort> OwnStudentIds =>
            User.FindAll("student_id")
                .Select(c => ushort.TryParse(c.Value, out var id) ? id : (ushort)0)
                .Where(id => id != 0)
                .ToHashSet();

        protected ushort? CurrentUserId =>
            ushort.TryParse(User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
                ? id
                : null;
    }
}
