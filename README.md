# MboaPrice

Application mobile .NET MAUI de comparaison des prix des denrées sur les marchés de Douala, avec son backend.

```
MboaPrice.sln
├── MboaPrice/        Application mobile (.NET 8 MAUI, MVVM) — maquettes Figma
└── MboaPrice.Api/    Backend (ASP.NET Core 8 + Entity Framework Core + SQLite + JWT)
```

## Lancer le backend
```
cd MboaPrice.Api
dotnet run
```
(ou double-clic sur `RUN_API.bat`). L'API écoute sur `http://0.0.0.0:5080` ; la documentation interactive est sur
**http://localhost:5080/swagger**. Au premier lancement, la base SQLite `mboaprice.db` est créée et remplie
(6 denrées, 4 marchés, 40 jours de relevés).

Compte de démonstration : `ariane.ndom@exemple.com` / `Demo1234`

## Lancer l'application
1. Démarrer l'API (ci-dessus).
2. **Émulateur Android** : rien à régler (l'app joint le PC via `10.0.2.2`).
   **Vrai téléphone** : même Wi-Fi que le PC, puis dans `MboaPrice/Helpers/AppConfig.cs` renseigner
   `PcLanIp = "192.168.x.x"` (IP du PC, commande `ipconfig`) et autoriser le port 5080 dans le pare-feu Windows.
3. Lancer `MboaPrice` sur Android.

Si l'API est éteinte ou injoignable, l'application bascule automatiquement sur des **données de démonstration**
(un message l'indique) : elle reste utilisable pour une présentation.

## Comment fonctionne le backend
Les utilisateurs envoient des **relevés** (prix vu chez un vendeur). L'API calcule tout le reste :
- prix moyen = moyenne des relevés des 3 derniers jours ; variation = comparaison avec il y a 7 à 9 jours ;
- prix par marché, marché le moins cher, historique quotidien (30 jours) pour la courbe ;
- protections : 20 relevés/jour/utilisateur, rejet des prix aberrants (> 5× la moyenne), pas de date future.

| Endpoint | Rôle | Écran |
|---|---|---|
| `POST /api/auth/register`, `POST /api/auth/login` | Compte + jeton JWT | Inscription, Connexion |
| `GET /api/overview` | Toutes les données de l'app en un appel | Accueil, Catalogue, Détail, Comparer, Tendances |
| `GET /api/commodities?category=&search=` | Liste filtrable | Catalogue |
| `GET /api/commodities/{id}` · `/history?days=` | Détail, historique | Détail, Tendances |
| `GET /api/markets` | Marchés | Ajouter un relevé |
| `POST /api/reports` 🔒 · `GET /api/me/reports` 🔒 | Publier / lister ses relevés | Ajouter un relevé, Profil |
| `GET/PUT/DELETE /api/me/favorites/...` 🔒 | Favoris | Favoris |
| `GET /api/me` 🔒 | Profil | Profil |

🔒 = jeton `Authorization: Bearer ...` requis. Les prix sont consultables sans compte (« Explorer sans compte »).

## Côté application (MboaPrice/)
- `Views/` + `ViewModels/` : une page XAML et un ViewModel par écran (10 écrans).
- `Controls/` : composants réutilisables (en-tête, carte denrée, champs, pastilles, barre de navigation, courbe).
- `Services/` : `ApiClient` (HTTP + JWT), `AuthService`, `PriceService` (cache + bascule démo), `DemoData`.
- `Helpers/AppConfig.cs` : adresse de l'API et option `StartupDiagnostics`.
- `Resources/Styles/` : couleurs et styles extraits de Figma. Icônes : SVG Lucide en 5 teintes (`nom_teinte.svg`).

### Diagnostic de démarrage
`AppConfig.StartupDiagnostics = true` crée chaque page au lancement : en cas d'erreur XAML, le détail s'affiche à
l'écran au lieu d'un écran blanc. Mettre `false` pour la version finale.

## Pour aller plus loin
- Migrations EF Core (`dotnet ef migrations add Init`) à la place de `EnsureCreated()` quand le schéma évolue.
- Changer `Jwt:Key` dans `appsettings.json` (clé secrète) avant tout déploiement ; héberger en HTTPS.
- Photos de relevés : stockage de fichiers côté API (pas encore implémenté).
