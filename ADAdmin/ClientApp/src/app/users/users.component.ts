import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserObject } from '../objects/user.object';
import { MatInput, MatPaginator, MatTable, MatTableDataSource, MatDialog } from '@angular/material';
import { PasswordResetComponent } from '../Dialogs/password-reset/password-reset.component';
import { getBaseUrl } from '../../main';
import { environment } from '../../environments/environment';
import { InfoComponent } from '../Dialogs/info/info.component';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit {

    @ViewChild(MatTable, { static: false }) table: MatTable<any>;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatInput, { static: false }) input: MatInput;

    commentOk = '';
    commentUnavailable = 'Serwer niedostępny, spróbuj ponownie później.';
    commentFailure = 'Operacja nie udała się.';
    commentNoAccess = 'Brak dostępu.';

    users: UserObject[] = [];
    dataSource: MatTableDataSource<UserObject>;
    displayedColumns: string[] = ['name', 'func_unlock', 'func_resetSession', 'func_resetPassword', 'func_status'];
    comment: string;
    currentVersion: number;
    filterType: string;

    constructor(public dialog: MatDialog, private router: Router, private http: HttpClient) {
        this.filterType = 'własny';
        this.comment = '';
    }

    async ngOnInit() {
        if (sessionStorage.getItem('Identity') === null)
            this.router.navigate(['/']);
        else
            await this.refresh();  
    }

    async refresh() {
        let filterValue;
        let filterPredicate;

        if (this.dataSource !== undefined) {
            filterPredicate = this.dataSource.filterPredicate;
            filterValue = this.dataSource.filter.valueOf();
        }

        await this.getUsersList().catch();

        if (filterValue !== null && filterValue !== undefined) {
            this.dataSource.filterPredicate = filterPredicate;
            this.dataSource.filter = filterValue;
        }
        
    }

    async getUsersList(): Promise<boolean> {
        let url;
        return new Promise<boolean>(async (resolve, reject) => {
            url = getBaseUrl() + environment.urlConstant + 'Users/List';
            this.users = [];

            await this.http.get(url, { withCredentials: true }).toPromise().then(
                (res: any[]) => {
                    if (res !== null) {
                        res.forEach(user => {
                            this.users.push((new UserObject(user)));
                        });
                    }
                    this.comment = this.commentOk;
                    this.dataSource = new MatTableDataSource<UserObject>(this.users);
                    this.dataSource.paginator = this.paginator;
                    resolve(true);
                },
                (res) => {
                    this.SetComment(res.status);
                    reject(false);
                });
        });
    }

    private SetComment(status: number) {
        if (status === 403)
            this.comment = this.commentNoAccess;
        else if (status === 422)
            this.comment = this.commentFailure
        else
            this.comment = this.commentUnavailable
    }

    async unlock(object: UserObject) {
        this.comment = this.commentOk;
        const url = getBaseUrl() + environment.urlConstant + 'Users/Unlock';
        var formData = new FormData();
        formData.append('username', object.name);

        await this.http.post<boolean>(url, formData, { withCredentials: true }).toPromise().then(
            async () => {
                await this.refresh();
            }, (res) => {
                this.SetComment(res.status);
            }
        );
    }

    async resetPassword(object: UserObject) {
        let password = '';
        const dialogRef = this.dialog.open(PasswordResetComponent, {
            data: 'Zmiana hasła użytkownika [' + object.name + ']'
        });

        dialogRef.afterClosed().subscribe(
            result => {
                password = result;
            },
            () => {
            },
            async () => {
                if (password) {
                    await this.postResetPassword(object, password);
                }
            });

    }

    async postResetPassword(object: UserObject, password: string) {
        this.comment = this.commentOk;
        const url = getBaseUrl() + environment.urlConstant + 'Users/ResetPassword';
        var formData = new FormData();
        formData.append('username', object.name);
        formData.append('password', password);

        await this.http.post<boolean>(url, formData, { withCredentials: true }).toPromise().then(
            async () => {
                await this.refresh();
            }, (res) => {
                this.SetComment(res.status);
            }
        );
    }

    async resetSession(object: UserObject) {
        this.comment = this.commentOk;
        const url = getBaseUrl() + environment.urlConstant + 'Users/ResetSession';
        var formData = new FormData();
        formData.append('username', object.name);

        await this.http.post<boolean>(url, formData, { withCredentials: true }).toPromise().then(
            async () => {
                await this.refresh();
            }, (res) => {
                this.SetComment(res.status);
            }
        );
    }

    async getStatus(object: UserObject) {
        this.comment = this.commentOk;
        const url = getBaseUrl() + environment.urlConstant + 'Users/GetStatus?username=' + object.name;
        let status;
        let success;

        await this.http.get<string[]>(url, { withCredentials: true }).toPromise().then(
            (res: string[]) => {
                if (res) {
                    status = res;
                }
                else {
                    status = ["Użytkownik nie logował się w ciągu ostatnich 24 godzin"];
                }
                success = true;
            },
            (res) => {
                this.SetComment(res.status)
                success = false;
            })

        if(success === true)
          this.dialog.open(InfoComponent, {
              data: { status, username: object.name }
          });
    }

    applyFilter(filterValue: string) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
