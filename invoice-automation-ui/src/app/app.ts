import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Loader } from './components/loader/loader';
import { Header } from './components/header/header';

@Component({
  imports: [RouterOutlet, Loader, Header],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('invoice-automation-ui');
}
