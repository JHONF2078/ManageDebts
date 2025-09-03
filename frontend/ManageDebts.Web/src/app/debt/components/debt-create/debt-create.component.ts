import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MATERIAL_IMPORTS } from '../../../material/material.component';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DebtService, CreateDebtDto } from '../../services/debt.service';
import { UsersService, User } from '../../../auth/services/users.service';

@Component({
  selector: 'app-debt-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './debt-create.component.html',
  styleUrl: './debt-create.component.scss'
})
export class DebtCreateComponent implements OnInit {
  debtForm: FormGroup;
  dateFormat: string = 'dd/MM/yyyy';

  customerName: string = '';
  users: { value: string; viewValue: string }[] = [];

  constructor(
    public dialogRef: MatDialogRef<DebtCreateComponent>,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private debtService: DebtService,
    private usersService: UsersService
  ) {

    this.debtForm = this.fb.group({
      amount: ['', [Validators.required, Validators.min(1)]],
      description: ['', Validators.required],
      creditorId: ['', Validators.required]
    });
  }

  ngOnInit() {

    this.usersService.getUsers().subscribe((users: any[]) => {
      //this.users = users.map(user => ({ value: user.id, viewValue: user.fullName }));
      this.users = users.map((users: { id: any; fullName: any; }) => {
        return { value: users.id, viewValue: `${users.fullName}` };
      });
    });
  }

  saveDebt(): void {
    if (this.debtForm.valid) {
      const debt: CreateDebtDto = this.debtForm.value;
      this.debtService.createDebt(debt).subscribe({
        next: (created: any) => {
          this.snackBar.open('Deuda creada exitosamente!', 'Cerrar', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
          });
          this.dialogRef.close(created);
        },
        error: (error) => {
          this.snackBar.open('Error al crear la deuda', 'Cerrar', {
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
    // Método 'onCancel' que cierra el diálogo sin retornar valores.
  }
}

