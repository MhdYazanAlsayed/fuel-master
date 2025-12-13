import Arabic from 'app/core/resources/arabic.json';
import English from 'app/core/resources/english.json';
import { Languages } from 'app/core/enums/Languages';
import Services from 'app/core/utilities/Services';
import WebService from 'app/core/abstracts/webService';

export default class LanguageService extends WebService {
  resources;
  isRTL;
  reRenderMethod;
  reRenderState;

  constructor() {
    super();
    this._localStorage = this.getService(Services.LocalStorageService);
    this.resources = null;
    this.isRTL = null;
    this.reRenderMethod = null;
  }

  /**
   * Change the language and RTL direction
   * @param {string} lang - The language to change to
   */

  changeLanguage(lang) {
    const isRTL = lang === Languages.Arabic;

    this.setLanguage(isRTL ? Arabic : English, isRTL);
    this._localStorage.setItem('language', lang);

    // Triger event to refresh compnents
    if (this.reRenderMethod) this.reRenderMethod();
  }

  /**
   * Set the language resources and RTL direction
   * @param {Object} resources - The language resources
   * @param {boolean} isRTL - The RTL direction
   */
  setLanguage(resources, isRTL) {
    this.resources = resources;
    this.isRTL = isRTL;
  }

  /**
   * Load the language from the local storage
   */
  load() {
    const data = this._localStorage.getItem('language');
    if (!data) {
      this.setLanguage(English, false);
      return;
    }

    const isRTL = data === Languages.Arabic;
    this.setLanguage(isRTL ? Arabic : English, isRTL);
  }

  /**
   * Configure the language service to re-render the components
   * @param {Function} reRenderMethod - The method to re-render the components
   */
  configure(reRenderState, reRenderMethod) {
    this.reRenderState = reRenderState;
    this.reRenderMethod = reRenderMethod;
  }
}
