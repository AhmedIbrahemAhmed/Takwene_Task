# AI-Assisted Development Decisions

## 1. What AI generated vs. what I wrote or modified

**I did myself:**
- The three-layer architecture decision (API / BLL / DAL) and the folder/project structure
- All NuGet and npm package choices
- Reviewed every generated file for correctness, security, and consistency with the layering rules (e.g., confirming DAL never references BLL/Api, controllers stay thin)
- Manual end-to-end testing of every endpoint via Swagger/cURL and the frontend, after each build phase

**AI generated (as a starting point, then reviewed):**
- Controllers (Artists, Tracks, DSPs, Auth) and their routing
- JWT token generation and bearer validation setup
- `AppDbContext`, entity configurations, and repository implementations
- Service layer implementations (Artist, Track, DSP, Distribution)
- DTOs and custom exceptions (`ValidationException`, `NotFoundException`, `ConflictException`)
- Angular component structure, services, and routing
- First draft of this README

**I rewrote or fixed:**
- Several `using`/namespace errors and duplicate namespace declarations (SeedData vs. DbContext)
- Seed data that tried to insert explicit IDs into identity columns — removed so SQL Server auto-increments correctly
- The cascade-delete configuration (see #3 below)
- Deprecated Swagger/OpenAPI setup calls and a couple of `Program.cs` methods that no longer matched the current .NET 8 minimal hosting APIs — updated to their current equivalents
- String → enum conversion for `TrackStatus`: the generated `TrackService` code treated status as a raw string when filtering tracks and when updating status, with no real validation that the value mapped to a valid enum — fixed with explicit `Enum.TryParse` and a validation error on mismatch (see #3 for detail)
- Angular zoneless migration: converted zone-based components to signals-based change detection, since the generated code assumed a zone-based setup
- Various missing brackets/semicolons in generated files

---

## 2. Security issues found and how I handled them

**Hardcoded credentials in `AuthController`.** The generated login check used a hardcoded `admin`/`admin123` comparison. Acceptable for this task's scope (no user store was required), but I documented it clearly as test-only in the README rather than letting it look like a real auth system — a production version needs a user store with hashed passwords.

**JWT secret placeholder in `appsettings.json`.** The app currently runs fine with the placeholder key as-is — there's no startup validation rejecting a weak or default secret, which I'm noting here as an unaddressed gap rather than something I fixed. I documented in the README that this key must be manually replaced with a real secret (and moved to user-secrets/an environment variable) before any non-local use, but didn't add enforcement given the time budget. A production version should fail startup if the key is missing or below a minimum length, rather than silently accepting it.

**Unrestricted cascade delete on Artist → Track.** Covered in detail in #3 — this was the most significant issue, since it could silently destroy distribution history.

**Unvalidated status strings in `TrackService`.** Both the status filter and the `PATCH /status` endpoint accepted any string and only worked correctly by coincidence when it happened to match an enum name's casing — there was no real validation rejecting invalid status values. This is a data-integrity issue as much as a security one: an invalid or malformed status could silently pass through instead of returning a clear 400. Fixed with explicit `Enum.TryParse` and a proper validation error (see #3).

**CORS scoped to one origin.** Left restrictive (`http://localhost:4200` only) rather than `AllowAnyOrigin`, which is what some generated CORS snippets default to. Documented that this is a dev-only value and would need to be environment-driven in production.

---

## 3. One thing AI got wrong that I had to fix

**Cascade delete on the Artist → Track relationship.**

```csharp
// AI-generated
modelBuilder.Entity<Track>()
    .HasOne(t => t.Artist)
    .WithMany(a => a.Tracks)
    .OnDelete(DeleteBehavior.Cascade);
```

This meant deleting a single artist would silently delete every one of their tracks, and by extension every `TrackDistribution` record tied to those tracks — including tracks already live on DSPs. That's a real data-integrity risk: an accidental or mistaken artist deletion wipes out distribution history with no confirmation step and no way to recover it.

Fixed by switching to `Restrict`:
```csharp
modelBuilder.Entity<Track>()
    .HasOne(t => t.Artist)
    .WithMany(a => a.Tracks)
    .OnDelete(DeleteBehavior.Restrict);
```
Now deleting an artist with existing tracks fails at the database level, forcing an explicit decision (reassign or delete tracks first) rather than an implicit cascade. I applied the same `Restrict` pattern to the DSP → TrackDistribution relationship for the same reason.

This was a good illustration of the AI generating something that *compiles and works in the happy path* but encodes a business decision it had no way of knowing was wrong — cascade delete is a completely reasonable EF Core default, it's just the wrong one for records representing licensed, DSP-facing distribution data.

**A second AI mistake, in `TrackService`:** the generated code for filtering tracks by status and for `PATCH /tracks/{id}/status` treated `status` as a raw string throughout — comparing it directly against the database instead of parsing it into the `TrackStatus` enum first. This meant filtering (`?status=Draft`) and status updates worked by accident when the casing happened to match, but would silently fail (return nothing, or save an invalid value) on any casing mismatch or typo, since there was no actual validation that the string mapped to a real enum value. I fixed this by explicitly parsing the incoming string with `Enum.TryParse<TrackStatus>(status, ignoreCase: true, out var parsed)` and returning a validation error when it doesn't match a known status, both in the filter path and the update path.

---

## Key takeaway

AI was fast and reliable for boilerplate — entities, repositories, controllers, DTOs — but every business rule embedded in that boilerplate (delete behavior, status transitions, credential handling) needed a human decision. The cascade-delete case in particular is the clearest example: it's syntactically correct, idiomatic EF Core, and the wrong choice for this domain. That's the gap I spent most of my review time on.

---

**Date**: August 15, 2026
**Project**: Music Distribution Platform
**AI tools used**: GitHub Copilot, Claude Sonnet 5, ChatGPT