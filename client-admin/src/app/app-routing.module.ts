import { CampaignComponent } from './pages/campaign/campaign.component';
import { CampaignListComponent } from './pages/campaign-list/campaign-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuardService } from './shared/auth-guard.service';
import { MenuRoles } from '../../src/app/models/enums';

import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { UserListComponent } from './pages/user-list/user-list.component';
import { UserComponent } from './pages/user/user.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { AccessDeniedComponent } from './pages/access-denied/access-denied.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'access-denied',
    component: AccessDeniedComponent,
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuardService],
    children: [
      {
        path: '',
        component: DashboardComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.TASKS,
        },
      },
      {
        path: 'user-list',
        component: UserListComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.USERS,
        },
      },
      {
        path: 'user',
        component: UserComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.USERS_CREATE,
        },
      },
      {
        path: 'campaigns',
        component: CampaignListComponent,
        //canActivate: [AuthGuardService],
        data: {
          //TODO: expectedRoles: MenuRoles.USERS_UPDATE,
        },
      },
      {
        path: 'campaigns/:id',
        component: CampaignComponent,
        //canActivate: [AuthGuardService],
        data: {
          //TODO: expectedRoles: MenuRoles.USERS_UPDATE,
        },
      },
      {
        path: 'user/:id/change-password',
        component: ChangePasswordComponent,
        canActivate: [AuthGuardService],
        data: {},
      },
    ],
  },
  // {
  //   path: 'campaignlist/',
  //   component: CampaignListComponent,
  //   canActivate: [AuthGuardService],
  //   data: {},
  // },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
