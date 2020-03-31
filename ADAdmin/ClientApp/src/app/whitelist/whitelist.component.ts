import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog, MatInput, MatPaginator, MatTable, MatTableDataSource } from '@angular/material';
import { AddressObject } from '../objects/address.object';
import { HttpClient } from '@angular/common/http';
import { ConfirmComponent } from '../Dialogs/confirm/confirm.component';
import { AskForValueComponent } from '../Dialogs/ask-for-value/ask-for-value.component';
import { getBaseUrl } from '../../main';
import { environment } from '../../environments/environment';

@Component({
    selector: 'app-whitelist',
    templateUrl: './whitelist.component.html',
    styleUrls: ['./whitelist.component.scss']
})
export class WhitelistComponent implements OnInit {

    @ViewChild(MatTable, { static: false }) table: MatTable<any>;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatInput, { static: false }) input: MatInput;

    commentOk = '';
    commentUnavailable = 'Serwer niedostępny, spróbuj ponownie później.';
    commentFailure = 'Operacja nie udała się';
    commentNoAccess = 'Brak dostępu.';

    addresses: AddressObject[] = [];
    dataSource: MatTableDataSource<AddressObject>;
    displayedColumns: string[] = ['path', 'func_remove'];
    comment: string;
    currentVersion: number;

    mode: number;

    constructor(public dialog: MatDialog, private router: Router, private http: HttpClient) {
        this.mode = 0;
        this.comment = '';
    }

    async ngOnInit() {
        if (sessionStorage.getItem('Identity') === null)
            this.router.navigate(['/']);

    }

    async refresh() {
        let filterValue;
        let filterPredicate;

        if (this.dataSource !== undefined) {
            filterPredicate = this.dataSource.filterPredicate;
            filterValue = this.dataSource.filter.valueOf();
        }

        await this.getAddressesList().catch();

        if (filterValue !== null && filterValue !== undefined) {
            this.dataSource.filterPredicate = filterPredicate;
            this.dataSource.filter = filterValue;
        }
    }

    async getAddressesList(): Promise<boolean> {
        let url;
        return new Promise<boolean>(async (resolve, reject) => {
            if (this.mode === 1) {
                url = getBaseUrl() + environment.urlConstant + 'Whitelist/AddressList';
            } else if (this.mode === 2) {
                url = getBaseUrl() + environment.urlConstant + 'Whitelist/DomainList';
            } else {
                reject(true);
            }
            this.addresses = [];

            await this.http.get(url, { withCredentials: true }).toPromise().then(
                (res: any[]) => {
                    if (res !== null) {
                        res.forEach(address => {
                            this.addresses.push((new AddressObject(address)));
                        });
                    }
                    this.comment = this.commentOk;
                    this.dataSource = new MatTableDataSource<AddressObject>(this.addresses);
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

    async showWhitelistAddress() {
        this.mode = 1;
        await this.refresh();
    }

    async showWhitelistDomain() {
        this.mode = 2;
        await this.refresh();
    }

    async remove(object: AddressObject) {
        let confirmed;
        const dialogRef = this.dialog.open(ConfirmComponent, {
            data: 'Czy na pewno chcesz usunąć [' + object.path + ']'
        });

        dialogRef.afterClosed().subscribe(
            result => {
                confirmed = result;
            },
            () => {
            },
            async () => {
                if (confirmed) {
                    await this.postRemove(object);
                }
            });

    }

    async postRemove(object: AddressObject) {
        this.comment = this.commentOk;

        let url;
        if (this.mode === 1) {
            url = getBaseUrl() + environment.urlConstant + 'Whitelist/AddressRemove';
        } else if (this.mode === 2) {
            url = getBaseUrl() + environment.urlConstant + 'Whitelist/DomainRemove';
        } else {
            return;
        }

        var formData = new FormData();
        formData.append('path', object.path);
        await this.http.post<boolean>(url, formData, { withCredentials: true }).toPromise().then(
            async () => {
                await this.refresh();
            }, (res) => {
                this.SetComment(res.status);
            }
        );
    }

    async add() {
        let path;
        const dialogRef = this.dialog.open(AskForValueComponent, {
            data: 'Podaj adres/domenę'
        });

        dialogRef.afterClosed().subscribe(
            result => {
                path = result;
            },
            () => {
            },
            async () => {
                if (path) {
                    await this.postAdd(path);
                }
            });
    }

    async postAdd(path: string) {
        this.comment = this.commentOk;

        let url;
        if (this.mode === 1) {
            url = getBaseUrl() + environment.urlConstant + 'Whitelist/AddressAdd';
        } else if (this.mode === 2) {
            url = getBaseUrl() + environment.urlConstant + 'Whitelist/DomainAdd';
        } else {
            return;
        }
        var formData = new FormData();
        formData.append('path', path);

        await this.http.post<boolean>(url, formData, { withCredentials: true }).toPromise().then(
            async () => {
                await this.refresh();
            }, (res) => {
                this.SetComment(res.status);
            }
        );
    }

    applyFilter(filterValue: string) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }
}
