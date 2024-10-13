import React, { lazy } from 'react';
import {
  createBrowserRouter,
  RouterProvider,
} from 'react-router-dom';
import Layout from '../Layout';

const Home = lazy(() => import('../../routes/Home'));
const Settings = lazy(() => import('../../routes/Settings'));
const About = lazy(() => import('../../routes/About'));

const router = createBrowserRouter([
  {
    element: <Layout />,
    children: [
      {
        index: true,
        path: '/',
        element: <Home />,
      },
      {
        path: '/settings',
        element: <Settings />,
      },
      {
        path: '/about',
        element: <About />,
      },
    ],
  },
]);

// 4️⃣ RouterProvider added
const App = () => <RouterProvider router={router} />;

export default App;
