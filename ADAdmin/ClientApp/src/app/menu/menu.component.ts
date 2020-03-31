import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { getBaseUrl } from '../../main';
import { environment } from '../../environments/environment';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

    title: string;
    credits: string;
    identity: string;

    constructor(private router: Router, private http: HttpClient) {
        this.title = 'AD ADMIN';
        this.credits = 'Copyright © 2019 - Professoft - ADAdmin.Client - 2019.2';
    }

    async ngOnInit() {
        let url = getBaseUrl() + environment.urlConstant + 'Permission/Check';
        await this.http.get(url, { withCredentials: true }).toPromise().then(
            async () => {
                url = getBaseUrl() + environment.urlConstant + 'Permission/Identity';
                await this.http.get(url, { withCredentials: true }).subscribe(
                    (res: string) => {
                        sessionStorage.setItem('Identity', res);
                        this.identity = res;
                    })
            },
            () => {
                //this.router.navigate(['/error']);
               
            });
    }
}
