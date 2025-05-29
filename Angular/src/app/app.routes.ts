import { provideRouter, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MainComponent } from './components/main/main.component';
import { appConfig } from './app.config';
import { ContributeComponent } from './components/contribute/contribute.component';
import { ProgressComponent } from './components/progress/progress.component';
export const routes: Routes = [
    {path:'', component: LoginComponent},
    {path:'main', component: MainComponent},
    {path:'contribute', component:ContributeComponent},
    {path:'progress', component:ProgressComponent}
];
export const appRouter=provideRouter(routes);
