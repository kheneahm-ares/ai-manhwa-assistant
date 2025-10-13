# Technical Specification - Service Technologies

## Service Architecture Overview

The Manhwa Reading Assistant implements a microservices architecture with Backend for Frontend (BFF) pattern, utilizing polyglot persistence and event-driven communication. The platform integrates with external manhwa APIs for content data while maintaining its own user tracking and AI recommendation systems.

## Technology Stack by Service

### 1. Next.js BFF Service (Frontend Aggregation Layer)

**Core Technologies:**
- **Framework**: Next.js 14+ with App Router and Server Components
- **Language**: TypeScript 5.0+ with strict configuration
- **Runtime**: Node.js 20+ LTS with performance optimizations
- **Authentication**: NextAuth.js v5 with OAuth 2.0 providers (Google, Discord, GitHub)
- **Server State Management**: TanStack Query (React Query) v5 for intelligent server state caching and synchronization
- **HTTP Client**: Native fetch with retry logic and request deduplication
- **Validation**: Zod v3 for request/response schema validation

**Key Responsibilities:**
- Session management and authentication proxy for all services
- Request aggregation and orchestration from multiple microservices
- Client-specific data transformation and caching with TanStack Query
- Intelligent server state management with automatic cache invalidation
- Optimistic UI updates for real-time progress tracking
- Error normalization and user-friendly error handling with automatic retries
- Static asset serving and SEO optimization for content pages
- Rate limiting coordination for external API calls

**Production Configuration:**
- **Port**: 3000
- **Replicas**: 3 with session affinity via Redis
- **Resource Limits**: 1Gi memory, 500m CPU
- **Environment Variables**: OAuth secrets, service discovery endpoints, Redis connection

**TanStack Query Configuration:**
TanStack Query serves as the intelligent server state management layer, handling all data fetching, caching, and synchronization from backend microservices.

```typescript
// Global Query Client Configuration
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 5 * 60 * 1000, // 5 minutes default cache
      gcTime: 10 * 60 * 1000, // 10 minutes garbage collection
      retry: 3, // Auto-retry failed requests
      refetchOnWindowFocus: true, // Refetch when user returns to tab
      refetchOnReconnect: true, // Refetch after network reconnection
    },
    mutations: {
      retry: 1, // Retry mutations once on failure
    },
  },
});

**Dependencies:**
```json
{
  "next": "^14.0.0",
  "next-auth": "^5.0.0",
  "zod": "^3.22.4",
  "@next/bundle-analyzer": "^14.0.0",
  "redis": "^4.6.10"
}
```

### 2. User Management Service (.NET Core)

**Core Technologies:**
- **Framework**: ASP.NET Core 8.0 with Minimal APIs
- **Language**: C# 12 with nullable reference types enabled
- **Database**: PostgreSQL 15+ with Entity Framework Core 8
- **Authentication**: JWT Bearer tokens with refresh token rotation
- **Authorization**: Policy-based authorization with custom requirements
- **Caching**: Distributed caching with Redis and in-memory fallback
- **API Documentation**: Swagger/OpenAPI 3.0 with comprehensive schemas

**Key Responsibilities:**
- User registration, authentication, and profile management
- Reading preferences and notification settings management
- User analytics and reading statistics aggregation
- Social features including following, blocking, and privacy settings
- Integration with external OAuth providers
- User data privacy and GDPR compliance management

**Database Schema:**
```sql
-- Core user management tables
Users (Id, Email, Username, PasswordHash, CreatedAt, LastLoginAt)
UserProfiles (UserId, DisplayName, AvatarUrl, Bio, Timezone, Language)
UserPreferences (UserId, NotificationSettings, PrivacySettings, ReadingPreferences)
UserSessions (Id, UserId, RefreshToken, ExpiresAt, DeviceInfo)
UserStats (UserId, TotalSeries, TotalChapters, ReadingStreak, JoinDate)
SocialConnections (UserId, FollowedUserId, ConnectionType, CreatedAt)
```

**Dependencies:**
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Npgsql" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity" Version="2.2.0" />
<PackageReference Include="StackExchange.Redis" Version="2.7.4" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
```

### 3. Reading Progress Service (.NET Core)

**Core Technologies:**
- **Framework**: ASP.NET Core 8.0 with Entity Framework Core
- **Language**: C# 12 with performance-optimized data access patterns
- **Primary Database**: PostgreSQL 15+ for transactional consistency
- **Cache Layer**: Redis Cluster for hot data and real-time synchronization
- **Real-time Updates**: SignalR with Azure Service Bus backplane
- **Background Processing**: Hangfire with PostgreSQL storage

**Key Responsibilities:**
- Chapter and episode progress tracking with timestamp precision
- Cross-device synchronization with conflict resolution algorithms
- Reading list management (Plan to Read, Currently Reading, Completed, Dropped)
- Bookmark management with notes and page/chapter references
- Reading session analytics and time tracking
- Progress sharing and social reading features

**Data Models and Schema:**
```sql
ReadingProgress (Id, UserId, ManhwaId, ChapterNumber, PageNumber, ProgressPercentage, LastUpdated, DeviceId)
ReadingLists (Id, UserId, ManhwaId, Status, Score, StartDate, CompletedDate, Notes)
Bookmarks (Id, UserId, ManhwaId, ChapterNumber, PageNumber, Note, CreatedAt)
ReadingSessions (Id, UserId, ManhwaId, StartTime, EndTime, ChaptersRead, DeviceType)
ProgressSyncEvents (Id, UserId, EventType, EventData, ProcessedAt, ConflictResolution)
```

**Advanced Features:**
- Conflict resolution for simultaneous updates across devices
- Reading streak calculation and gamification elements
- Progress prediction algorithms based on reading patterns
- Batch progress updates for offline synchronization

**Dependencies:**
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Npgsql" Version="8.0.0" />
<PackageReference Include="StackExchange.Redis" Version="2.7.4" />
<PackageReference Include="Microsoft.AspNetCore.SignalR" Version="1.1.0" />
<PackageReference Include="Hangfire.PostgreSql" Version="1.20.8" />
<PackageReference Include="MediatR" Version="12.2.0" />
<PackageReference Include="FluentValidation" Version="11.8.0" />
```

### 4. Content Service (Python FastAPI)

**Core Technologies:**
- **Framework**: FastAPI 0.104+ with async/await throughout
- **Language**: Python 3.11+ with type hints and Pydantic v2
- **Database**: MongoDB 7.0+ for flexible schema and rapid iteration
- **Search Engine**: Elasticsearch 8.0+ for full-text search and analytics
- **HTTP Client**: httpx with connection pooling and retry mechanisms
- **Caching**: Redis with intelligent cache invalidation
- **External APIs**: GraphQL client for AniList, REST client for MangaDx

**Key Responsibilities:**
- Manhwa metadata management and normalization across sources
- External API integration with rate limiting and circuit breakers
- Content search functionality with advanced filtering and faceting
- Manhwa discovery algorithms and trending content identification
- Content similarity analysis using NLP and metadata comparison
- Korean content handling including romanization and cultural context

**Data Models:**
```python
class ManhwaMetadata(BaseModel):
    id: str
    title: str
    alternative_titles: List[str]
    korean_title: Optional[str]
    romanized_title: str
    description: str
    genres: List[str]
    demographics: List[str]  # Shounen, Seinen, etc.
    manhwa_specific_tags: List[str]  # System, Regression, Murim, etc.
    status: ContentStatus
    chapter_count: Optional[int]
    episode_count: Optional[int]
    publication_year: Optional[int]
    rating: Optional[float]
    popularity_rank: Optional[int]
    cover_image: str
    banner_image: Optional[str]
    external_ids: Dict[str, str]  # anilist_id, mangadx_id, etc.
    cultural_context: Optional[List[str]]
    content_warnings: List[str]
    reading_difficulty: Optional[str]
    last_updated: datetime

class SearchFilters(BaseModel):
    genres: Optional[List[str]] = None
    demographics: Optional[List[str]] = None
    status: Optional[List[ContentStatus]] = None
    year_range: Optional[Tuple[int, int]] = None
    rating_range: Optional[Tuple[float, float]] = None
    tags: Optional[List[str]] = None
    sort_by: Optional[str] = "popularity"
    sort_order: Optional[str] = "desc"
```

**External API Integration:**
```python
# AniList GraphQL Integration
class AniListClient:
    async def fetch_manhwa_by_id(self, anilist_id: int) -> ManhwaMetadata
    async def search_manhwa(self, query: str, filters: SearchFilters) -> List[ManhwaMetadata]
    async def fetch_trending_manhwa(self, time_period: str = "WEEK") -> List[ManhwaMetadata]

# MangaDx REST Integration  
class MangaDxClient:
    async def fetch_manga_by_id(self, mangadx_id: str) -> ManhwaMetadata
    async def search_manga(self, query: str, filters: Dict) -> List[ManhwaMetadata]
    async def fetch_chapter_list(self, mangadx_id: str) -> List[ChapterInfo]
```

**Dependencies:**
```python
# requirements.txt
fastapi==0.104.1
uvicorn[standard]==0.24.0
motor==3.3.2  # Async MongoDB driver
elasticsearch==8.11.0
httpx==0.25.2
pydantic==2.5.0
redis==5.0.1
aiofiles==23.2.1
python-multipart==0.0.6
```

### 5. AI/ML Recommendation Service (Python FastAPI)

**Core Technologies:**
- **Framework**: FastAPI 0.104+ with background task processing
- **Language**: Python 3.11+ optimized for ML workloads
- **ML Libraries**: scikit-learn 1.3+, pandas 2.1+, numpy 1.24+
- **Advanced ML**: PyTorch 2.1+ for deep learning models (Phase 3)
- **Vector Database**: Elasticsearch with dense vector support for embeddings
- **Cache**: Redis for model predictions and feature caching
- **Background Processing**: Celery with Redis broker for model training
- **NLP Processing**: transformers library for Korean text analysis

**Key Responsibilities:**
- Collaborative filtering using matrix factorization and neural collaborative filtering
- Content-based filtering using TF-IDF, word embeddings, and genre similarity
- Hybrid recommendation system combining multiple approaches
- A/B testing framework for recommendation algorithm evaluation
- Real-time recommendation API with sub-500ms response times
- Model training pipelines and performance monitoring

**ML Models and Algorithms:**
```python
# Collaborative Filtering Models
class MatrixFactorizationModel:
    """SVD-based collaborative filtering for user-item interactions"""
    def __init__(self, n_factors: int = 50, learning_rate: float = 0.01)
    def fit(self, user_item_matrix: np.ndarray) -> None
    def predict(self, user_id: int, item_ids: List[int]) -> List[float]

class NeuralCollaborativeFiltering:
    """Deep learning approach using neural networks"""
    def __init__(self, embedding_dim: int = 64, hidden_layers: List[int] = [128, 64])
    def train(self, interactions: pd.DataFrame) -> None
    def recommend(self, user_id: int, n_recommendations: int = 10) -> List[Tuple[int, float]]

# Content-Based Filtering
class ContentBasedRecommender:
    """Uses manhwa metadata and description analysis"""
    def __init__(self, use_transformers: bool = True)
    def build_content_profiles(self, manhwa_data: List[ManhwaMetadata]) -> None
    def recommend_similar(self, manhwa_id: str, n_recommendations: int = 10) -> List[str]

# Hybrid System
class HybridRecommendationEngine:
    """Combines multiple recommendation approaches with learned weights"""
    def __init__(self, collaborative_model, content_model, popularity_model)
    def predict_preferences(self, user_id: str, candidate_items: List[str]) -> Dict[str, float]
    def generate_recommendations(self, user_id: str, filters: Optional[Dict] = None) -> List[RecommendationResult]
```

**AI Summary Generation:**
```python
class AIContentSummarizer:
    """Generates summaries and recaps using LLM integration"""
    
    async def generate_catch_up_summary(
        self, 
        manhwa_id: str, 
        last_read_chapter: int, 
        current_chapter: int,
        user_preferences: UserPreferences
    ) -> SummaryResult:
        """Generate 'Where Was I?' summaries for returning readers"""
    
    async def generate_series_recap(
        self,
        manhwa_id: str,
        spoiler_level: SpoilerLevel = SpoilerLevel.NONE
    ) -> SeriesRecap:
        """Generate series overview for recommendations"""
    
    async def detect_hiatus_and_summarize(
        self,
        manhwa_id: str,
        user_id: str
    ) -> Optional[HiatusRecap]:
        """Detect hiatuses and generate recovery summaries"""
```

**Dependencies:**
```python
# requirements.txt
fastapi==0.104.1
scikit-learn==1.3.2
pandas==2.1.4
numpy==1.24.3
torch==2.1.0  # For advanced models
transformers==4.35.0  # For Korean text processing
redis==5.0.1
celery==5.3.4
motor==3.3.2
elasticsearch==8.11.0
pydantic==2.5.0
openai==1.3.0  # For GPT integration
langchain==0.0.335  # For LLM orchestration
```

### 6. Analytics and Notification Service (.NET Core)

**Core Technologies:**
- **Framework**: ASP.NET Core 8.0 with Minimal APIs
- **Language**: C# 12 with async/await patterns
- **Message Queue**: RabbitMQ with MassTransit for reliable messaging
- **Database**: MongoDB for notification templates and delivery logs
- **Email Service**: SendGrid with template management
- **Real-time**: SignalR for browser notifications
- **Background Jobs**: Hangfire for scheduled notifications and analytics

**Key Responsibilities:**
- Event-driven notification processing from all services
- Email notifications for chapter releases, recommendations, and community activity
- Real-time browser notifications with user preference management
- Analytics data collection and processing
- User engagement metrics and reading pattern analysis
- Notification delivery optimization and A/B testing

**Event-Driven Architecture:**
```csharp
// Event Types
public record ChapterReleaseEvent(string ManhwaId, int ChapterNumber, DateTime ReleaseDate);
public record UserProgressUpdatedEvent(string UserId, string ManhwaId, int ChapterNumber);
public record RecommendationGeneratedEvent(string UserId, List<RecommendationItem> Recommendations);

// Event Handlers
public class ChapterReleaseNotificationHandler : IConsumer<ChapterReleaseEvent>
{
    public async Task Consume(ConsumeContext<ChapterReleaseEvent> context)
    {
        // Send notifications to users tracking this manhwa
        // Respect user notification preferences and timing
        // Track delivery success and user engagement
    }
}
```

**Dependencies:**
```xml
<PackageReference Include="MassTransit.RabbitMQ" Version="8.1.3" />
<PackageReference Include="MongoDB.Driver" Version="2.22.0" />
<PackageReference Include="SendGrid" Version="9.29.2" />
<PackageReference Include="Microsoft.AspNetCore.SignalR" Version="1.1.0" />
<PackageReference Include="Hangfire.Mongo" Version="1.10.4" />
<PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.21.0" />
```

## Infrastructure and Deployment Technologies

### Container Orchestration
```yaml
# Local Development
platform: minikube
container_runtime: docker
orchestration: kubernetes_1.28
service_mesh: istio_1.19  # For advanced networking

# Production Options
cloud_providers:
  - azure_kubernetes_service  # AKS
  - amazon_elastic_kubernetes_service  # EKS  
  - google_kubernetes_engine  # GKE
```

### Database Configuration
```yaml
# Production Database Setup
postgresql:
  version: "15.4"
  configuration: "Primary/Standby with 2 read replicas"
  connection_pooling: "PgBouncer"
  backup: "Point-in-time recovery with 7-day retention"
  
mongodb:
  version: "7.0"
  configuration: "Replica Set (Primary + 2 Secondary)"
  sharding: "Enabled for manhwa content collection"
  backup: "Automated daily snapshots"

redis:
  version: "7.2"
  configuration: "Cluster mode with 6 nodes (3 primary + 3 replica)"
  persistence: "RDB + AOF for durability"
  
elasticsearch:
  version: "8.11"
  configuration: "3-node cluster with dedicated master"
  indices: "Separate indices for manhwa content and user analytics"
```

### Observability and Monitoring Stack
```yaml
metrics:
  collection: prometheus
  storage: prometheus_tsdb
  visualization: grafana
  alerting: alertmanager

tracing:
  collection: opentelemetry
  storage: jaeger
  sampling: "1% for high-volume endpoints, 100% for errors"

logging:
  collection: fluent-bit
  processing: elasticsearch
  visualization: kibana
  retention: "30 days for debug logs, 1 year for audit logs"

apm:
  dotnet: application_insights
  python: opentelemetry_python
  custom_metrics: "Recommendation accuracy, API latency, user engagement"
```

This technical specification provides comprehensive implementation guidance while maintaining clear separation between services and their specific technological requirements. Each service is optimized for its domain responsibilities while contributing to the overall AI-powered manhwa tracking platform.