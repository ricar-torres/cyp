import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuardService } from './shared/auth-guard.service';
import { MenuRoles } from '../../src/app/models/enums';

import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { UserListComponent } from './pages/user-list/user-list.component';
import { UserComponent } from './pages/user/user.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FileComponent } from './pages/file/file.component';
import { FileListComponent } from './pages/file-list/file-list.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { DocumentTypeListComponent } from './pages/document-type-list/document-type-list.component';
import { DocumentTypeComponent } from './pages/document-type/document-type.component';
import { AccessDeniedComponent } from './pages/access-denied/access-denied.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login/0',
    pathMatch: 'full',
  },
  {
    path: 'login/:app',
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
        path: 'file-list',
        component: FileListComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.FILES,
        },
      },
      {
        path: 'file/:id',
        component: FileComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.FILES,
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
        path: 'document-type',
        component: DocumentTypeComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.DOCUMENT_TYPES_CREATE,
        },
      },
      {
        path: 'document-type/:id',
        component: DocumentTypeComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.DOCUMENT_TYPES_UPDATE,
        },
      },
      {
        path: 'document-type-list',
        component: DocumentTypeListComponent,
        canActivate: [AuthGuardService],
        data: {
          expectedRoles: MenuRoles.DOCUMENT_TYPES,
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
