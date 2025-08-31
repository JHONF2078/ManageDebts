import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MATERIAL_IMPORTS } from '../../../material/material.component';
import { DebtService, CreateDebtDto } from '../../services/debt.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UsersService, User } from '../../../auth/services/users.service';

@Component({
  selector: 'app-debt-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './debt-edit.component.html',
  styleUrl: './debt-edit.component.scss'
})
export class DebtEditComponent {
  users: User[] = [];
  debtForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<DebtEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private debtService: DebtService,
    private snackBar: MatSnackBar,
    private usersService: UsersService,
  ) {
    this.debtForm = this.fb.group({
      amount: [data.amount, [Validators.required, Validators.min(1)]],
      description: [data.description, Validators.required],
      creditorId: [data.creditorId, Validators.required]
    });

    this.usersService.getUsers().subscribe(users => {
      this.users = users;
    });
  }

  save(): void {
    if (this.debtForm.valid) {
      const updatedDebt: CreateDebtDto = this.debtForm.value;
      this.debtService.updateDebt(this.data.id, updatedDebt).subscribe({
        next: (created) => {
          this.snackBar.open('Deuda Editada exitosamente!', 'Cerrar', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
          });
          this.dialogRef.close(created);
        },
        error: (err) => {
          const msg = err?.error || 'Error al Editar la deuda';

          this.snackBar.open(msg, 'Cerrar', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
          });
        }
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
