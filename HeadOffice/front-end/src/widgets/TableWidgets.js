import React from 'react';
import { Card, Col, Row } from 'react-bootstrap';
import WidgetSectionTitle from './WidgetSectionTitle';
import RecentPurchases from 'components/theme/dashboards/e-commerce/recent-purchases/RecentPurchases';
import Customers from 'components/theme/app/e-commerce/customers/Customers';
import SharedFiles from 'components/theme/dashboards/default/SharedFiles';
import TopPages from 'components/theme/dashboards/analytics/top-pages/TopPages';
import { topPagesTableData, intelligence } from 'data/dashboard/analytics';
import { files } from 'data/dashboard/default';
import {
  membersActivities,
  recentActivities,
  membersInfo,
  runningProjects
} from 'data/dashboard/projectManagement';
import { transactionSummary } from 'data/dashboard/saas';
import Experience from 'components/theme/pages/user/Experience';
import experiences from 'data/experiences';
import ToDoList from 'components/theme/dashboards/project-management/ToDoList';
import MemberInfo from 'components/theme/dashboards/project-management/MemberInfo';
import MembersActivity from 'components/theme/dashboards/project-management/MembersActivity';
import Intelligence from 'components/theme/dashboards/analytics/Intelligence';
import CampaignPerfomance from 'components/theme/dashboards/analytics/campaign-perfomance/CampaignPerfomance';
import RunningProjects from 'components/theme/dashboards/project-management/RunningProjects';
import TransactionSummary from 'components/theme/dashboards/saas/TransactionSummary';
import RecentActivity from 'components/theme/dashboards/project-management/RecentActivity';

const TableWidgets = () => {
  return (
    <>
      <WidgetSectionTitle
        icon="list"
        title="Tables, Files, and Lists"
        r
        subtitle="Falcon's styled components are dedicatedly made for displaying your contents and lists."
        transform="shrink-2"
        className="mb-4 mt-6"
      />
      <div className="mb-3">
        <RecentPurchases />
      </div>
      <div className="mb-3">
        <Customers />
      </div>

      <Row className="g-3 mb-3">
        <Col lg={6}>
          <SharedFiles files={files} />
        </Col>
        <Col lg={6}>
          <Card className="h-100">
            <Card.Header className="bg-light">
              <h5 className="mb-0">Experience</h5>
            </Card.Header>
            <Card.Body className="fs--1">
              {experiences.map((experience, index) => (
                <Experience
                  key={experience.id}
                  experience={experience}
                  isLast={index === experiences.length - 1}
                />
              ))}
            </Card.Body>
          </Card>
        </Col>
        <Col lg={7} xxl={8}>
          <TopPages tableData={topPagesTableData} perPage={6} />
        </Col>
        <Col lg={5} xxl={4}>
          <ToDoList />
        </Col>
      </Row>

      <Row className="g-3 mb-3">
        <Col lg={8}>
          <MemberInfo data={membersInfo} />
        </Col>

        <Col lg={4}>
          <MembersActivity data={membersActivities} />
        </Col>
      </Row>

      <Row className="g-3 mb-3">
        <Col xxl={8}>
          <TransactionSummary data={transactionSummary} />
        </Col>

        <Col xxl={4}>
          <RecentActivity data={recentActivities} />
        </Col>
      </Row>

      <Row className="g-3 mb-3">
        <Col md={6} xxl={5}>
          <Intelligence data={intelligence} />
        </Col>
        <Col md={6} xxl={7}>
          <CampaignPerfomance />
        </Col>
      </Row>
      <RunningProjects data={runningProjects} />
    </>
  );
};

export default TableWidgets;
