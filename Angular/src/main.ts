import { provideHttpClient } from "@angular/common/http";
import { bootstrapApplication } from "@angular/platform-browser";
import { AppComponent } from "./app/app.component";
import { appRouter } from "./app/app.routes";
import { AuthService } from "./app/services/auth.service";
bootstrapApplication(AppComponent, {
  providers: [
    appRouter,             // ✅ Provide routes properly
    provideHttpClient(),  // ✅ Provide HTTP client service
    AuthService
  ]
}).catch((err) => console.error(err));
