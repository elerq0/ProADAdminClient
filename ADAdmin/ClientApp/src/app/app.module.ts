import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpUrlEncodingCodec, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import {
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatButtonToggleModule, MatMenuModule, MatDialogModule
} from '@angular/material';

import { AppComponent } from './app.component';
import { UsersComponent } from './users/users.component';
import { WhitelistComponent } from './whitelist/whitelist.component';
import { MenuComponent } from './menu/menu.component';
import { ConfirmComponent } from './Dialogs/confirm/confirm.component';
import { PasswordResetComponent } from './Dialogs/password-reset/password-reset.component';
import { AskForValueComponent } from './Dialogs/ask-for-value/ask-for-value.component';
import { NoAccessComponent } from './no-access/no-access.component';
import { InfoComponent } from './Dialogs/info/info.component';


@NgModule({
    declarations: [
        AppComponent,
        UsersComponent,
        WhitelistComponent,
        MenuComponent,
        ConfirmComponent,
        PasswordResetComponent,
        AskForValueComponent,
        NoAccessComponent,
        InfoComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        BrowserAnimationsModule,
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: MenuComponent, },
            { path: 'users', component: UsersComponent },
            { path: 'whitelist', component: WhitelistComponent },
            { path: 'error', component: NoAccessComponent},
        ], { useHash: true }),
        MatButtonModule,
        MatButtonToggleModule,
        MatInputModule,
        MatDialogModule,
        MatFormFieldModule,
        MatMenuModule,
        MatTableModule,
        MatPaginatorModule,

    ],
    entryComponents: [
        PasswordResetComponent,
        ConfirmComponent,
        AskForValueComponent,
        InfoComponent,
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
