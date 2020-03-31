import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'app-password-reset',
    templateUrl: './password-reset.component.html'
})
export class PasswordResetComponent {

    comment = '';
    password: string;
    confirmation: string;

    constructor(
        public dialogRef: MatDialogRef<PasswordResetComponent>,
        @Inject(MAT_DIALOG_DATA) public message: string) {
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    onChange(password: string, confirmation: string): void {
        if (password === confirmation) {
            this.comment = '';
            this.dialogRef.close(password);
        } else {
            this.comment = 'Hasła różnią się!';
        }
    }
}
