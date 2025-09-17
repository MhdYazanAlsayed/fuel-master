import Arabic from '../core/resources/arabic.json';
import English from '../core/resources/english.json';
import { Languages } from '../core/enums/Languages';

export default class LanguageService {
  resources;
  isRTL;
  reRenderMethod;
  renderState;

  constructor(_localStorage) {
    this._localStorage = _localStorage;
    this.resources = null;
    this.isRTL = null;
    this.reRenderMethod = null;
  }

  changeLanguage(lang) {
    const isRTL = lang === Languages.Arabic;

    this.setLanguage(isRTL ? Arabic : English, isRTL);
    this._localStorage.setItem('language', lang);

    // Triger event to refresh compnents
    if (this.reRenderMethod) this.reRenderMethod();
    if (this.changeRTLDirection) this.changeRTLDirection(isRTL);
  }

  setLanguage(resources, isRTL) {
    this.resources = resources;
    this.isRTL = isRTL;
  }

  load() {
    const data = this._localStorage.getItem('language');
    if (!data) {
      this.setLanguage(English, false);
      return;
    }

    const isRTL = data === Languages.Arabic;
    this.setLanguage(isRTL ? Arabic : English, isRTL);
  }

  configure(renderState, reRenderMethod) {
    this.reRenderMethod = reRenderMethod;
    this.renderState = renderState;
  }
}
