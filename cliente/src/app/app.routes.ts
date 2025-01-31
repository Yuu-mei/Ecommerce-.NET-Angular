import { Routes } from '@angular/router';
import { authGuard, orderGuard } from './auth/guards/auth.guard';
import { VideogameListComponent } from './videogames/videogame-list/videogame-list.component';
import { adminGuard } from './auth/guards/admin.guard';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';
import { AdminVideogameListComponent } from './admin/admin-videogame-list/admin-videogame-list.component';

export const routes: Routes = [
    {
        path: "",
        pathMatch: "full",
        redirectTo: "videogame-list"
    },
    {
        path: "videogame-list",
        // This one isn't lazy loaded as it is the one that appears from the getgo
        component: VideogameListComponent
    },
    {
        path: "videogame/:id",
        loadComponent: () => import("./videogames/videogame-detail/videogame-detail.component").then((m) => m.VideogameDetailComponent)
    },
    {
        path: "cart",
        loadComponent: () => import("./cart/cart.component").then((m) => m.CartComponent),
        canActivate: [authGuard]
    },
    {
        path: "orders",
        loadComponent: () => import("./videogame-order/videogame-order.component").then((m) => m.VideogameOrderComponent),
        canActivate: [orderGuard]
    },
    {
        path: 'search',
        loadComponent: () => import("./search/search.component").then((m) => m.SearchComponent),
    },
    {
        // This could also be a children of the search component but preferred to separate them so as to avoid doing another component that is search results
        path:"advanced_search",
        loadComponent: () => import("./search/advancedsearch/advancedsearch.component").then((m) => m.AdvancedSearchComponent)
    },
    {
        path: 'login',
        loadComponent: () => import("./auth/login/login.component").then((m) => m.LoginComponent)
    },
    {
        path: 'signup',
        loadComponent: () => import("./auth/signup/signup.component").then((m) => m.SignupComponent)
    },
    {
        path: 'profile',
        loadComponent: () => import("./auth/profile/profile.component").then((m) => m.ProfileComponent),
        canActivate: [authGuard]
    },
    {
        path: "login-admin",
        loadComponent: () => import("./auth/login-admin/login-admin.component").then((m) => m.LoginAdminComponent)
    },
    {
        path: "admin",
        canActivateChild: [adminGuard],
        children: [
            {
                path: "",
                pathMatch: "full",
                loadComponent:  () => import("./admin/admin-dashboard/admin-dashboard.component").then((m) => m.AdminDashboardComponent)
            },
            {
                path: "videogame-list",
                loadComponent: () => import("./admin/admin-videogame-list/admin-videogame-list.component").then((m) => m.AdminVideogameListComponent)
            },
            {
                path: "add-videogame",
                loadComponent: () => import("./admin/admin-add-videogame/admin-add-videogame.component").then((m) => m.AdminAddVideogameComponent)
            },
            {
                path: "edit-videogame/:id",
                loadComponent: () => import("./admin/admin-edit-videogame/admin-edit-videogame.component").then((m) => m.AdminEditVideogameComponent)
            },
            {
                path: "user-list",
                loadComponent: () => import("./admin/admin-user-list/admin-user-list.component").then((m) => m.AdminUserListComponent)
            },
            {
                path: "add-user",
                loadComponent: () => import("./admin/admin-add-user/admin-add-user.component").then((m) => m.AdminAddUserComponent)
            },
            {
                path: "edit-user/:id",
                loadComponent: () => import("./admin/admin-edit-user/admin-edit-user.component").then((m) => m.AdminEditUserComponent)
            },
            {
                path: "order-list",
                loadComponent: () => import("./admin/admin-order-list/admin-order-list.component").then((m) => m.AdminOrderListComponent)
            },
            {
                path: "order/:id",
                loadComponent: () => import("./admin/admin-detail-order/admin-detail-order.component").then((m) => m.AdminDetailOrderComponent)
            }
        ]
    }
];
