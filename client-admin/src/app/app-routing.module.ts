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
import { AgencyListComponent } from './pages/agencies/agency-list/agency-list.component';
import { AgencyComponent } from './pages/agencies/agency/agency.component';
import { BonaFideListComponent } from './pages/bona-fide-list/bona-fide-list.component';
import { BonaFideComponent } from './pages/bona-fide/bona-fide.component';
import { ChapterComponent } from './pages/chapter/chapter.component';
import { ChapterListComponent } from './pages/chapter-list/chapter-list.component';

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
          //expectedRoles: [],
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
        path: 'user/:id',
        component: UserComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.USERS_UPDATE,
        },
      },
      {
        path: 'user/:id/change-password',
        component: ChangePasswordComponent,
        canActivate: [AuthGuardService],
        data: {},
      },
      {
        path: 'agencies',
        component: AgencyListComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.AGENCIES,
        },
      },
      {
        path: 'agency',
        component: AgencyComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.AGENCIES_CREATE,
        },
      },
      {
        path: 'agency/:id',
        component: AgencyComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.AGENCIES_UPDATE,
        },
      },
      {
        path: 'bonafides',
        component: BonaFideListComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.BONAFIDE_CREATE,
        },
      },
      {
        path: 'bonafide',
        component: BonaFideComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.BONAFIDE_CREATE,
        },
      },
      {
        path: 'bonafide/:id',
        component: BonaFideComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.BONAFIDE_UPDATE,
        },
      },
      {
        path: 'chapters',
        component: ChapterListComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.CHAPTER_CREATE,
        },
      },
      {
        path: 'chapter',
        component: ChapterComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.CHAPTER_CREATE,
        },
      },
      {
        path: 'chapter/:id',
        component: ChapterComponent,
        canActivate: [AuthGuardService],
        data: {
          //expectedRoles: MenuRoles.CHAPTER_UPDATE,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
