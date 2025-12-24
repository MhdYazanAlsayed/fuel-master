import React, { useContext, useEffect } from 'react';
import { Navigate, Route, Routes } from 'react-router-dom';
import AuthSimpleLayout from './AuthSimpleLayout';
import is from 'is_js';
import MainLayout from './MainLayout';
import SettingsPanel from 'components/theme/settings-panel/SettingsPanel';

import ErrorLayout from './ErrorLayout';
import WizardAuth from 'components/theme/authentication/wizard/WizardAuth';
import Landing from 'components/theme/pages/landing/Landing';
import { toast, ToastContainer } from 'react-toastify';
import { CloseButton } from 'components/theme/common/Toast';

import Accordion from 'components/theme/doc-components/Accordion';
import Alerts from 'components/theme/doc-components/Alerts';
import Badges from 'components/theme/doc-components/Badges';
import Breadcrumbs from 'components/theme/doc-components/Breadcrumb';
import Buttons from 'components/theme/doc-components/Buttons';
import CalendarExample from 'components/theme/doc-components/CalendarExample';
import Cards from 'components/theme/doc-components/Cards';
import Dropdowns from 'components/theme/doc-components/Dropdowns';
import ListGroups from 'components/theme/doc-components/ListGroups';
import Modals from 'components/theme/doc-components/Modals';
import Offcanvas from 'components/theme/doc-components/Offcanvas';
import Pagination from 'components/theme/doc-components/Pagination';
import BasicProgressBar from 'components/theme/doc-components/ProgressBar';
import Spinners from 'components/theme/doc-components/Spinners';
import Toasts from 'components/theme/doc-components/Toasts';
import Avatar from 'components/theme/doc-components/Avatar';
import Image from 'components/theme/doc-components/Image';
import Tooltips from 'components/theme/doc-components/Tooltips';
import Popovers from 'components/theme/doc-components/Popovers';
import Figures from 'components/theme/doc-components/Figures';
import Hoverbox from 'components/theme/doc-components/Hoverbox';
import Tables from 'components/theme/doc-components/Tables';
import FormControl from 'components/theme/doc-components/FormControl';
import InputGroup from 'components/theme/doc-components/InputGroup';
import Select from 'components/theme/doc-components/Select';
import Checks from 'components/theme/doc-components/Checks';
import Range from 'components/theme/doc-components/Range';
import FormLayout from 'components/theme/doc-components/FormLayout';
import FloatingLabels from 'components/theme/doc-components/FloatingLabels';
import FormValidation from 'components/theme/doc-components/FormValidation';
import BootstrapCarousel from 'components/theme/doc-components/BootstrapCarousel';
import SlickCarousel from 'components/theme/doc-components/SlickCarousel';
import Navs from 'components/theme/doc-components/Navs';
import Navbars from 'components/theme/doc-components/Navbars';
import Tabs from 'components/theme/doc-components/Tabs';
import Collapse from 'components/theme/doc-components/Collapse';
import CountUp from 'components/theme/doc-components/CountUp';
import Embed from 'components/theme/doc-components/Embed';
import Background from 'components/theme/doc-components/Backgrounds';
import Search from 'components/theme/doc-components/Search';
import VerticalNavbar from 'components/theme/doc-components/VerticalNavbar';
import NavBarTop from 'components/theme/doc-components/NavBarTop';
import NavbarDoubleTop from 'components/theme/doc-components/NavbarDoubleTop';
import ComboNavbar from 'components/theme/doc-components/ComboNavbar';
import TypedText from 'components/theme/doc-components/TypedText';
import FileUploader from 'components/theme/doc-components/FileUploader';
import Borders from 'components/theme/utilities/Borders';
import Colors from 'components/theme/utilities/Colors';
import ColoredLinks from 'components/theme/utilities/ColoredLinks';
import Display from 'components/theme/utilities/Display';
import Visibility from 'components/theme/utilities/Visibility';
import StretchedLink from 'components/theme/utilities/StretchedLink';
import Float from 'components/theme/utilities/Float';
import Position from 'components/theme/utilities/Position';
import Spacing from 'components/theme/utilities/Spacing';
import Sizing from 'components/theme/utilities/Sizing';
import TextTruncation from 'components/theme/utilities/TextTruncation';
import Typography from 'components/theme/utilities/Typography';
import VerticalAlign from 'components/theme/utilities/VerticalAlign';
import Flex from 'components/theme/utilities/Flex';
import Grid from 'components/theme/utilities/Grid';
import WizardForms from 'components/theme/doc-components/WizardForms';
import GettingStarted from 'components/theme/documentation/GettingStarted';
import Configuration from 'components/theme/documentation/Configuration';
import DarkMode from 'components/theme/documentation/DarkMode';
import Plugins from 'components/theme/documentation/Plugins';
import Styling from 'components/theme/documentation/Styling';
import DesignFile from 'components/theme/documentation/DesignFile';
import Starter from 'components/theme/pages/Starter';
import AnimatedIcons from 'components/theme/doc-components/AnimatedIcons';
import DatePicker from 'components/theme/doc-components/DatePicker';
import FontAwesome from 'components/theme/doc-components/FontAwesome';
import Changelog from 'components/theme/documentation/change-log/ChangeLog';
import Analytics from 'components/theme/dashboards/analytics';
import Crm from 'components/theme/dashboards/crm';
import Saas from 'components/theme/dashboards/saas';
import Profile from 'components/theme/pages/user/profile/Profile';
import Associations from 'components/theme/pages/asscociations/Associations';
import Followers from 'components/theme/app/social/followers/Followers';
import Notifications from 'components/theme/app/social/notifications/Notifications';
import ActivityLog from 'components/theme/app/social/activity-log/ActivityLog';
import Settings from 'components/theme/pages/user/settings/Settings';
import Feed from 'components/theme/app/social/feed/Feed';
import Placeholder from 'components/theme/doc-components/Placeholder';
import Lightbox from 'components/theme/doc-components/Lightbox';
import AdvanceTableExamples from 'components/theme/doc-components/AdvanceTableExamples';
import ModalAuth from 'components/theme/authentication/modal/ModalAuth';
import Calendar from 'components/theme/app/calendar/Calendar';
import FaqAlt from 'components/theme/pages/faq/faq-alt/FaqAlt';
import FaqBasic from 'components/theme/pages/faq/faq-basic/FaqBasic';
import FaqAccordion from 'components/theme/pages/faq/faq-accordion/FaqAccordion';
import PrivacyPolicy from 'components/theme/pages/miscellaneous/privacy-policy/PrivacyPolicy';
import InvitePeople from 'components/theme/pages/miscellaneous/invite-people/InvitePeople';
import PricingDefault from 'components/theme/pages/pricing/pricing-default/PricingDefault';
import PricingAlt from 'components/theme/pages/pricing/pricing-alt/PricingAlt';
import Invoice from 'components/theme/app/e-commerce/Invoice';
import Billing from 'components/theme/app/e-commerce/billing/Billing';
import Checkout from 'components/theme/app/e-commerce/checkout/Checkout';
import ShoppingCart from 'components/theme/app/e-commerce/cart/ShoppingCart';
import CustomersDetails from 'components/theme/app/e-commerce/customers-details/CustomersDetails';
import OrderDetails from 'components/theme/app/e-commerce/orders/order-details/OrderDetails';
import Products from 'components/theme/app/e-commerce/product/Products';
import ProductDetails from 'components/theme/app/e-commerce/product/product-details/ProductDetails';
import Orders from 'components/theme/app/e-commerce/orders/order-list/Orders';
import Customers from 'components/theme/app/e-commerce/customers/Customers';
import Courses from 'components/theme/app/e-learning/course/Courses';
import CourseDetails from 'components/theme/app/e-learning/course/course-details';
import CreateCourse from 'components/theme/app/e-learning/course/create-a-course';
import TrainerProfile from 'components/theme/app/e-learning/trainer-profile';
import StudentOverview from 'components/theme/app/e-learning/student-overview';
import CreateEvent from 'components/theme/app/events/create-an-event/CreateEvent';
import EventList from 'components/theme/app/events/event-list/EventList';
import EventDetail from 'components/theme/app/events/event-detail/EventDetail';
import EmailDetail from 'components/theme/app/email/email-detail/EmailDetail';
import Compose from 'components/theme/app/email/compose/Compose';
import Inbox from 'components/theme/app/email/inbox/Inbox';
import Rating from 'components/theme/doc-components/Rating';
import AdvanceSelect from 'components/theme/doc-components/AdvanceSelect';
import Editor from 'components/theme/doc-components/Editor';
import Chat from 'components/theme/app/chat/Chat';
import Kanban from 'components/theme/app/kanban/Kanban';
import DraggableExample from 'components/theme/doc-components/DraggableExample';
import LineCharts from 'components/theme/doc-components/charts-example/echarts/line-charts';
import BarCharts from 'components/theme/doc-components/charts-example/echarts/bar-charts';
import CandlestickCharts from 'components/theme/doc-components/charts-example/echarts/candlestick-charts';
import GeoMaps from 'components/theme/doc-components/charts-example/echarts/geo-map';
import ScatterCharts from 'components/theme/doc-components/charts-example/echarts/scatter-charts';
import PieCharts from 'components/theme/doc-components/charts-example/echarts/pie-charts';
import RadarCharts from 'components/theme/doc-components/charts-example/echarts/radar-charts/Index';
import HeatmapCharts from 'components/theme/doc-components/charts-example/echarts/heatmap-chart';
import Chartjs from 'components/theme/doc-components/charts-example/chartjs';
import D3js from 'components/theme/doc-components/charts-example/d3';
import HowToUse from 'components/theme/doc-components/charts-example/echarts/HowToUse';
import GoogleMapExample from 'components/theme/doc-components/GoogleMapExample';
import LeafletMapExample from 'components/theme/doc-components/LeafletMapExample';
import CookieNoticeExample from 'components/theme/doc-components/CookieNoticeExample';
import Scrollbar from 'components/theme/doc-components/Scrollbar';
import Scrollspy from 'components/theme/doc-components/Scrollspy';
import ReactIcons from 'components/theme/doc-components/ReactIcons';
import ReactPlayerExample from 'components/theme/doc-components/ReactPlayerExample';
import EmojiMartExample from 'components/theme/doc-components/EmojiMart';
import TreeviewExample from 'components/theme/doc-components/TreeviewExample';
import Timeline from 'components/theme/doc-components/Timeline';
import Widgets from 'widgets/Widgets';
import Ecommerce from 'components/theme/dashboards/e-commerce';
import Lms from 'components/theme/dashboards/lms';
import ProjectManagement from 'components/theme/dashboards/project-management';

import Error404 from 'components/theme/errors/Error404';
import Error500 from 'components/theme/errors/Error500';

import SimpleLogin from 'components/theme/authentication/simple/Login';
import SimpleLogout from 'components/theme/authentication/simple/Logout';
import SimpleRegistration from 'components/theme/authentication/simple/Registration';
import SimpleForgetPassword from 'components/theme/authentication/simple/ForgetPassword';
import SimplePasswordReset from 'components/theme/authentication/simple/PasswordReset';
import SimpleConfirmMail from 'components/theme/authentication/simple/ConfirmMail';
import SimpleLockScreen from 'components/theme/authentication/simple/LockScreen';

import CardLogin from 'components/theme/authentication/card/Login';
import Logout from 'components/theme/authentication/card/Logout';
import CardRegistration from 'components/theme/authentication/card/Registration';
import CardForgetPassword from 'components/theme/authentication/card/ForgetPassword';
import CardConfirmMail from 'components/theme/authentication/card/ConfirmMail';
import CardPasswordReset from 'components/theme/authentication/card/PasswordReset';
import CardLockScreen from 'components/theme/authentication/card/LockScreen';

import SplitLogin from 'components/theme/authentication/split/Login';
import SplitLogout from 'components/theme/authentication/split/Logout';
import SplitRegistration from 'components/theme/authentication/split/Registration';
import SplitForgetPassword from 'components/theme/authentication/split/ForgetPassword';
import SplitPasswordReset from 'components/theme/authentication/split/PasswordReset';
import SplitConfirmMail from 'components/theme/authentication/split/ConfirmMail';
import SplitLockScreen from 'components/theme/authentication/split/LockScreen';

import Wizard from 'components/theme/wizard/Wizard';
import AppContext from 'context/Context';
import Faq from 'components/theme/documentation/Faq';

import SupportDesk from 'components/theme/dashboards/support-desk';
import TableView from 'components/theme/app/support-desk/tickets-layout/TableView';
import CardView from 'components/theme/app/support-desk/tickets-layout/CardView';
import Contacts from 'components/theme/app/support-desk/contacts/Contacts';
import ContactDetails from 'components/theme/app/support-desk/contact-details/ContactDetails';
import TicketsPreview from 'components/theme/app/support-desk/tickets-preview/TicketsPreview';
import QuickLinks from 'components/theme/app/support-desk/quick-links/QuickLinks';
import Reports from 'components/theme/app/support-desk/reports/Reports';
import Login from 'components/auth/Login';
import Cities from 'components/management/cities/Index';
import FuelTypes from 'components/management/fuel-types/Index';
import AuthorizedLayout from './AuthorizedLayout';
import Zones from 'components/management/zones/Index';
import ChangePrices from 'components/management/zones/prices/ChangePrices';
import Prices from 'components/management/zones/prices/Prices';
import Histories from 'components/management/zones/prices/Histories';
import Stations from 'components/management/stations/Index';
import Tanks from 'components/management/tanks/Index';
import Pumps from 'components/management/pumps/Index';
import Nozzles from 'components/management/nozzles/Index';
import CreateNozzle from 'components/management/nozzles/Create';
import EditNozzle from 'components/management/nozzles/Edit';
import CreateDelivery from 'components/management/deliveries/Create';
import Employees from 'components/user-management/employees/Index';
import CreateEmployee from 'components/user-management/employees/Create';
import Groups from 'components/user-management/groups/Index';
import CreateGroup from 'components/user-management/groups/Create';
import EditGroup from 'components/user-management/groups/Edit';
import EditEmployee from 'components/user-management/employees/Edit';
import MainDashboard from 'components/dashboard/Index';
import RealTime from 'components/reports/realtime/Index';
import Transaction from 'components/reports/transaction/Index';
import Deliveries from 'components/reports/deliveries/Index';
import Permissions from 'components/user-management/groups/permissions/Index';
import ProfileSettings from 'components/settings/Index';
import Time from 'components/reports/time/Index';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import Areas from 'components/management/areas/Index';
import Roles from 'components/user-management/roles/Index';
import CreateRole from 'components/user-management/roles/Create';
import EditRole from 'components/user-management/roles/Edit';
import RoleDetails from 'components/user-management/roles/Details';

const Layout = () => {
  const _languageService = useService(Services.LanguageService);

  const HTMLClassList = document.getElementsByTagName('html')[0].classList;
  // const _identityService = DependenciesInjector.services.identityService;

  const {
    config: { navbarPosition }
  } = useContext(AppContext);

  useEffect(() => {
    if (is.windows()) {
      HTMLClassList.add('windows');
    }
    if (is.chrome()) {
      HTMLClassList.add('chrome');
    }
    if (is.firefox()) {
      HTMLClassList.add('firefox');
    }
    if (is.safari()) {
      HTMLClassList.add('safari');
    }
  }, [HTMLClassList]);

  useEffect(() => {
    if (navbarPosition === 'double-top') {
      HTMLClassList.add('double-top-nav-layout');
    }
    return () => HTMLClassList.remove('double-top-nav-layout');
  }, [navbarPosition]);

  return (
    <>
      <Routes>
        <Route path="/account/login" element={<Login />} />
        <Route path="/account/logout" element={<Logout />} />

        <Route element={<MainLayout />}>
          <Route element={<AuthorizedLayout />}>
            {/* <Route path="/" element={<MainDashboard />} /> */}
            <Route path="/" element={<>Welcome !</>} />
            <Route path="/cities" element={<Cities />} />
            <Route path="/fuel-types" element={<FuelTypes />} />
            <Route path="/zones" element={<Zones />} />
            <Route path="/zones/:zoneId/prices" element={<Prices />} />
            <Route
              path="/zones/prices/:zonePriceId/histories"
              element={<Histories />}
            />

            <Route
              path="/zones/:zoneId/change-prices"
              element={<ChangePrices />}
            />

            <Route path="/stations" element={<Stations />} />
            <Route path="/tanks" element={<Tanks />} />
            <Route path="/pumps" element={<Pumps />} />

            <Route path="/nozzles" element={<Nozzles />} />
            <Route path="/nozzles/create" element={<CreateNozzle />} />
            <Route path="/nozzles/:id/edit" element={<EditNozzle />} />

            <Route path="/areas" element={<Areas />} />
            <Route path="/roles" element={<Roles />} />
            <Route path="/roles/create" element={<CreateRole />} />
            <Route path="/roles/:id/edit" element={<EditRole />} />
            <Route path="/roles/:id/details" element={<RoleDetails />} />
            <Route path="/employees" element={<Employees />} />
            <Route path="/employees/create" element={<CreateEmployee />} />
            <Route path="/employees/:id/edit" element={<EditEmployee />} />

            {/* 

        

            <Route path="/deliveries/create" element={<CreateDelivery />} />



            <Route path="/groups" element={<Groups />} />
            <Route path="/groups/create" element={<CreateGroup />} />
            <Route path="/groups/:id/edit" element={<EditGroup />} />
            <Route path="/groups/:id/permissions" element={<Permissions />} />*/}

            <Route path="/reports/realtime" element={<RealTime />} />
            <Route path="/reports/transactions" element={<Transaction />} />
            <Route path="/reports/deliveries" element={<Deliveries />} />
            <Route path="/reports/time" element={<Time />} />
            <Route path="/account/settings" element={<ProfileSettings />} />
          </Route>
        </Route>

        {/* =============================================================== */}
        {/* =============================================================== */}
        {/* =============================================================== */}
        {/* =============================================================== */}
        {/* =============================================================== */}
        {/* =============================================================== */}
        {/* ========================= Theme  ============================== */}
        {/* =============================================================== */}
        {/* =============================================================== */}
        {/* =============================================================== */}
        {/* =============================================================== */}
        {/* =============================================================== */}
        <Route path="landing" element={<Landing />} />
        <Route element={<ErrorLayout />}>
          <Route path="errors/404" element={<Error404 />} />
          <Route path="errors/500" element={<Error500 />} />
        </Route>
        {/*- ------------- Authentication ---------------------------  */}

        {/*- ------------- simple ---------------------------  */}
        <Route element={<AuthSimpleLayout />}>
          <Route path="authentication/simple/login" element={<SimpleLogin />} />
          <Route
            path="authentication/simple/register"
            element={<SimpleRegistration />}
          />
          <Route
            path="authentication/simple/logout"
            element={<SimpleLogout />}
          />
          <Route
            path="authentication/simple/forgot-password"
            element={<SimpleForgetPassword />}
          />
          <Route
            path="authentication/simple/reset-password"
            element={<SimplePasswordReset />}
          />
          <Route
            path="authentication/simple/confirm-mail"
            element={<SimpleConfirmMail />}
          />
          <Route
            path="authentication/simple/lock-screen"
            element={<SimpleLockScreen />}
          />
        </Route>

        {/*- ------------- Card ---------------------------  */}
        <Route path="authentication/card/login" element={<CardLogin />} />
        <Route
          path="authentication/card/register"
          element={<CardRegistration />}
        />
        <Route
          path="authentication/card/forgot-password"
          element={<CardForgetPassword />}
        />
        <Route
          path="authentication/card/reset-password"
          element={<CardPasswordReset />}
        />
        <Route
          path="authentication/card/confirm-mail"
          element={<CardConfirmMail />}
        />
        <Route
          path="authentication/card/lock-screen"
          element={<CardLockScreen />}
        />

        {/*- ------------- Split ---------------------------  */}

        <Route path="authentication/split/login" element={<SplitLogin />} />
        <Route path="authentication/split/logout" element={<SplitLogout />} />
        <Route
          path="authentication/split/register"
          element={<SplitRegistration />}
        />
        <Route
          path="authentication/split/forgot-password"
          element={<SplitForgetPassword />}
        />
        <Route
          path="authentication/split/reset-password"
          element={<SplitPasswordReset />}
        />
        <Route
          path="authentication/split/confirm-mail"
          element={<SplitConfirmMail />}
        />
        <Route
          path="authentication/split/lock-screen"
          element={<SplitLockScreen />}
        />

        <Route element={<WizardAuth />}>
          <Route
            path="authentication/wizard"
            element={<Wizard validation={true} />}
          />
        </Route>

        {/* //--- MainLayout Starts  */}

        <Route element={<MainLayout />}>
          {/*Dashboard*/}
          <Route path="dashboard/analytics" element={<Analytics />} />
          <Route path="dashboard/crm" element={<Crm />} />
          <Route path="dashboard/saas" element={<Saas />} />
          <Route path="dashboard/e-commerce" element={<Ecommerce />} />
          <Route path="dashboard/lms" element={<Lms />} />
          <Route
            path="dashboard/project-management"
            element={<ProjectManagement />}
          />
          <Route path="dashboard/support-desk" element={<SupportDesk />} />
          {/* E Commerce */}
          <Route
            path="e-commerce/orders/order-details"
            element={<OrderDetails />}
          />
          <Route path="e-commerce/orders/order-list" element={<Orders />} />
          <Route path="e-commerce/invoice" element={<Invoice />} />
          <Route path="e-commerce/billing" element={<Billing />} />
          <Route path="e-commerce/checkout" element={<Checkout />} />
          <Route path="e-commerce/shopping-cart" element={<ShoppingCart />} />
          <Route path="e-commerce/customers" element={<Customers />} />
          <Route
            path="e-commerce/customer-details"
            element={<CustomersDetails />}
          />

          <Route
            path="e-commerce/product/product-details"
            element={<ProductDetails />}
          />
          <Route
            path="e-commerce/product/product-details/:productId"
            element={<ProductDetails />}
          />

          <Route
            path="e-commerce/product/:productLayout"
            element={<Products />}
          />

          <Route path="e-commerce/invoice" element={<Invoice />} />

          {/* E Learning */}
          <Route path="e-learning/course/:courseLayout" element={<Courses />} />
          <Route
            path="e-learning/course/course-details"
            element={<CourseDetails />}
          />
          <Route
            path="e-learning/course/course-details/:courseId"
            element={<CourseDetails />}
          />
          <Route
            path="e-learning/course/create-a-course"
            element={<CreateCourse />}
          />
          <Route
            path="e-learning/trainer-profile"
            element={<TrainerProfile />}
          />
          <Route
            path="e-learning/student-overview"
            element={<StudentOverview />}
          />

          {/*icons*/}
          <Route path="icons/font-awesome" element={<FontAwesome />} />
          <Route path="icons/react-icons" element={<ReactIcons />} />

          {/* maps */}
          <Route path="maps/google-map" element={<GoogleMapExample />} />
          <Route path="maps/leaflet-map" element={<LeafletMapExample />} />

          {/*App*/}
          <Route path="app/calendar" element={<Calendar />} />
          <Route path="app/chat" element={<Chat />} />
          <Route path="app/kanban" element={<Kanban />} />
          <Route path="social/feed" element={<Feed />} />
          <Route path="social/activity-log" element={<ActivityLog />} />
          <Route path="social/notifications" element={<Notifications />} />
          <Route path="social/followers" element={<Followers />} />
          <Route path="events/event-detail" element={<EventDetail />} />
          <Route path="events/create-an-event" element={<CreateEvent />} />
          <Route path="events/event-list" element={<EventList />} />

          {/* Email */}
          <Route path="email/email-detail" element={<EmailDetail />} />
          <Route path="email/inbox" element={<Inbox />} />
          <Route path="email/compose" element={<Compose />} />

          {/* support desk */}
          <Route path="/support-desk/table-view" element={<TableView />} />
          <Route path="/support-desk/card-view" element={<CardView />} />
          <Route path="/support-desk/contacts" element={<Contacts />} />
          <Route
            path="/support-desk/contact-details"
            element={<ContactDetails />}
          />
          <Route
            path="/support-desk/tickets-preview"
            element={<TicketsPreview />}
          />
          <Route path="/support-desk/quick-links" element={<QuickLinks />} />
          <Route path="/support-desk/reports" element={<Reports />} />

          {/*Pages*/}
          <Route path="pages/starter" element={<Starter />} />
          <Route path="user/profile" element={<Profile />} />
          <Route path="user/settings" element={<Settings />} />
          <Route path="miscellaneous/associations" element={<Associations />} />
          <Route path="faq/faq-alt" element={<FaqAlt />} />
          <Route path="faq/faq-basic" element={<FaqBasic />} />
          <Route path="faq/faq-accordion" element={<FaqAccordion />} />
          <Route path="pricing/pricing-default" element={<PricingDefault />} />
          <Route path="pricing/pricing-alt" element={<PricingAlt />} />
          <Route
            path="miscellaneous/privacy-policy"
            element={<PrivacyPolicy />}
          />
          <Route
            path="miscellaneous/invite-people"
            element={<InvitePeople />}
          />
          {/* charts-example */}

          <Route path="charts/chartjs" element={<Chartjs />} />
          <Route path="charts/d3js" element={<D3js />} />
          <Route path="charts/echarts/line-charts" element={<LineCharts />} />
          <Route path="charts/echarts/bar-charts" element={<BarCharts />} />
          <Route
            path="charts/echarts/candlestick-charts"
            element={<CandlestickCharts />}
          />
          <Route path="charts/echarts/geo-map" element={<GeoMaps />} />
          <Route
            path="charts/echarts/scatter-charts"
            element={<ScatterCharts />}
          />
          <Route path="charts/echarts/pie-charts" element={<PieCharts />} />
          <Route path="charts/echarts/radar-charts" element={<RadarCharts />} />
          <Route
            path="charts/echarts/heatmap-charts"
            element={<HeatmapCharts />}
          />
          <Route path="charts/echarts/how-to-use" element={<HowToUse />} />

          {/*Components*/}
          <Route path="components/alerts" element={<Alerts />} />
          <Route path="components/accordion" element={<Accordion />} />
          <Route path="components/animated-icons" element={<AnimatedIcons />} />
          <Route path="components/badges" element={<Badges />} />
          <Route path="components/breadcrumb" element={<Breadcrumbs />} />
          <Route path="components/buttons" element={<Buttons />} />
          <Route path="components/calendar" element={<CalendarExample />} />
          <Route path="components/cards" element={<Cards />} />
          <Route path="components/dropdowns" element={<Dropdowns />} />
          <Route path="components/list-group" element={<ListGroups />} />
          <Route path="components/modals" element={<Modals />} />
          <Route path="components/offcanvas" element={<Offcanvas />} />
          <Route path="components/pagination" element={<Pagination />} />
          <Route
            path="components/progress-bar"
            element={<BasicProgressBar />}
          />
          <Route path="components/placeholder" element={<Placeholder />} />
          <Route path="components/spinners" element={<Spinners />} />
          <Route path="components/toasts" element={<Toasts />} />
          <Route path="components/pictures/avatar" element={<Avatar />} />
          <Route path="components/pictures/images" element={<Image />} />
          <Route path="components/pictures/figures" element={<Figures />} />
          <Route path="components/pictures/hoverbox" element={<Hoverbox />} />
          <Route path="components/pictures/lightbox" element={<Lightbox />} />
          <Route path="components/tooltips" element={<Tooltips />} />
          <Route path="components/popovers" element={<Popovers />} />
          <Route path="components/draggable" element={<DraggableExample />} />
          <Route path="components/scrollspy" element={<Scrollspy />} />
          <Route path="components/timeline" element={<Timeline />} />
          <Route path="components/treeview" element={<TreeviewExample />} />
          <Route
            path="components/carousel/bootstrap"
            element={<BootstrapCarousel />}
          />
          <Route path="components/carousel/slick" element={<SlickCarousel />} />
          <Route path="components/navs-and-tabs/navs" element={<Navs />} />
          <Route path="tables/basic-tables" element={<Tables />} />
          <Route
            path="tables/advance-tables"
            element={<AdvanceTableExamples />}
          />
          <Route path="forms/basic/form-control" element={<FormControl />} />
          <Route path="forms/basic/input-group" element={<InputGroup />} />
          <Route path="forms/basic/select" element={<Select />} />
          <Route path="forms/basic/checks" element={<Checks />} />
          <Route path="forms/basic/range" element={<Range />} />
          <Route path="forms/basic/layout" element={<FormLayout />} />
          <Route path="forms/advance/date-picker" element={<DatePicker />} />
          <Route path="forms/advance/editor" element={<Editor />} />
          <Route
            path="forms/advance/emoji-button"
            element={<EmojiMartExample />}
          />
          <Route
            path="forms/advance/advance-select"
            element={<AdvanceSelect />}
          />
          <Route
            path="forms/advance/file-uploader"
            element={<FileUploader />}
          />
          <Route path="forms/advance/rating" element={<Rating />} />
          <Route path="forms/floating-labels" element={<FloatingLabels />} />
          <Route path="forms/validation" element={<FormValidation />} />
          <Route path="forms/wizard" element={<WizardForms />} />
          <Route path="components/navs-and-tabs/navbar" element={<Navbars />} />
          <Route path="components/navs-and-tabs/tabs" element={<Tabs />} />
          <Route path="components/collapse" element={<Collapse />} />
          <Route
            path="components/cookie-notice"
            element={<CookieNoticeExample />}
          />
          <Route path="components/countup" element={<CountUp />} />
          <Route path="components/videos/embed" element={<Embed />} />
          <Route
            path="components/videos/react-player"
            element={<ReactPlayerExample />}
          />
          <Route path="components/background" element={<Background />} />
          <Route path="components/search" element={<Search />} />
          <Route path="components/typed-text" element={<TypedText />} />
          <Route
            path="components/navs-and-tabs/vertical-navbar"
            element={<VerticalNavbar />}
          />
          <Route
            path="components/navs-and-tabs/top-navbar"
            element={<NavBarTop />}
          />
          <Route
            path="components/navs-and-tabs/double-top-navbar"
            element={<NavbarDoubleTop />}
          />
          <Route
            path="components/navs-and-tabs/combo-navbar"
            element={<ComboNavbar />}
          />

          {/*Utilities*/}
          <Route path="utilities/borders" element={<Borders />} />
          <Route path="utilities/colors" element={<Colors />} />
          <Route path="utilities/colored-links" element={<ColoredLinks />} />
          <Route path="utilities/display" element={<Display />} />
          <Route path="utilities/visibility" element={<Visibility />} />
          <Route path="utilities/stretched-link" element={<StretchedLink />} />
          <Route path="utilities/stretched-link" element={<StretchedLink />} />
          <Route path="utilities/float" element={<Float />} />
          <Route path="utilities/position" element={<Position />} />
          <Route path="utilities/spacing" element={<Spacing />} />
          <Route path="utilities/sizing" element={<Sizing />} />
          <Route
            path="utilities/text-truncation"
            element={<TextTruncation />}
          />
          <Route path="utilities/typography" element={<Typography />} />
          <Route path="utilities/vertical-align" element={<VerticalAlign />} />
          <Route path="utilities/flex" element={<Flex />} />
          <Route path="utilities/grid" element={<Grid />} />
          <Route path="utilities/scroll-bar" element={<Scrollbar />} />

          <Route path="widgets" element={<Widgets />} />

          {/*Documentation*/}
          <Route
            path="documentation/getting-started"
            element={<GettingStarted />}
          />
          <Route
            path="documentation/configuration"
            element={<Configuration />}
          />
          <Route path="documentation/styling" element={<Styling />} />
          <Route path="documentation/dark-mode" element={<DarkMode />} />
          <Route path="documentation/plugin" element={<Plugins />} />
          <Route path="documentation/faq" element={<Faq />} />
          <Route path="documentation/design-file" element={<DesignFile />} />
          <Route path="changelog" element={<Changelog />} />
          <Route path="authentication-modal" element={<ModalAuth />} />
        </Route>

        {/* //--- MainLayout end  */}

        {/* <Navigate to="/errors/404" /> */}
        <Route path="*" element={<Navigate to="/errors/404" replace />} />
      </Routes>
      <SettingsPanel />
      <ToastContainer
        closeButton={CloseButton}
        icon={false}
        rtl={_languageService.isRTL}
        position={
          _languageService.isRTL
            ? toast.POSITION.BOTTOM_LEFT
            : toast.POSITION.BOTTOM_RIGHT
        }
      />
    </>
  );
};

export default Layout;
