Prompt: Implement Full CRUD Module (Service Layer + Soft Delete + File Handling + Pagination)

Attach these files before running: docs/architecture.json, docs/erd.json

Using the architecture and ERD context files, implement full {EntityName} module following Clean Architecture (WITHOUT CQRS) using Service Layer pattern only.

Follow existing solution structure exactly:

Domain
Application (Services + DTOs)
Infrastructure (EF + File system)
Web.Api (Controllers only)
📌 REQUIRED FEATURES (FOR EACH ENTITY)

Implement for {EntityName}:

1. CRUD OPERATIONS (via Service Layer)

Inside {EntityName}Service:

Create{Entity}
Update{Entity}
Delete{Entity} (Soft Delete only)
GetById{Entity}
GetAll{Entity} (non-deleted only)
GetPaged{Entity}
2. ACTIVE / INACTIVE

Add:

ToggleStatus method
Flip IsActive

Rules:

Default = true
Inactive records must not appear in public GetAll/GetPaged
3. SOFT DELETE RULE
NEVER physically delete database records
On delete:
IsDeleted = true
DeletedAt = UtcNow
All queries MUST filter:
IsDeleted == false
4. FILE HANDLING RULE (IMPORTANT)

Applies to:

Banner
Brand
Partner
Upload Rules:
Frontend sends: IFormFile Image
Backend must:

Save file to:

wwwroot/uploads/{entity}/
Generate GUID filename

Store ONLY relative path:

/uploads/{entity}/{fileName}
Update Rules:
If new file provided:
Delete old file physically
Upload new file
Update DB path
Delete Rules:
Before soft delete:
Delete physical file from wwwroot
5. PAGINATION ENDPOINT

Must support:

PageNumber
PageSize
Search (NameAr / NameEn if exists)
IsActive filter

Return:

PagedResult<T>
{
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
}
📌 SERVICE LAYER RULES

Each service must:

Be injected via DI
Use Repository pattern or DbContext via Infrastructure
NOT contain controller logic
NOT use CQRS
Handle all business logic inside service only
📌 ENTITY RULES
Banner
Id
ImageUrl
IsActive
Soft delete fields

Only one image

Brand
Id
NameAr
NameEn
ImageUrl
IsActive

Used for filtering products

Partner
Id
NameAr
NameEn
ImageUrl
IsActive

Display-only entity

📌 API CONTROLLER RULES

Controllers must:

Call service only
No business logic
No file handling
No DB access
📌 ENDPOINTS
POST   /api/{entity}
PUT    /api/{entity}
DELETE /api/{entity}/{id}
GET    /api/{entity}/{id}
GET    /api/{entity}
POST    /api/{entity}/paged
PUT  /api/{entity}/{id}/toggle-status
📌 VALIDATION RULES
NameAr / NameEn: required (where exists)
Image: required on create
File:
Allowed: jpg, jpeg, png, webp
Max: 10MB
📌 CHECKLIST
 Soft delete implemented
 No hard delete in DB
 File deletion on update/delete
 Pagination implemented
 Service layer contains all logic
 Controller is thin
 IsActive filtering applied
 No CQRS used anywhere
🎯 APPLY THIS PROMPT FOR:

Run 3 times:

Banner
Brand
Partner