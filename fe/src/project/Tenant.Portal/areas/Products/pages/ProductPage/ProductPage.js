import React from 'react';
import { ThemeProvider } from '@material-ui/core/styles';
import Fab from '@material-ui/core/Fab';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import { Button, TextField, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, TablePagination } from '@material-ui/core';
import { Plus } from 'react-feather';
import moment from 'moment';
import GlobalHelper from '../../../../../../shared/helpers/globals/GlobalHelper';
import PaginationConstants from '../../../../../../shared/constants/PaginationConstants';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */

function ProductPage(props) {

  const [page, setPage] = React.useState(2);
  const [rowsPerPage, setRowsPerPage] = React.useState(10);

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  return (
    <ThemeProvider theme={GlobalHelper.initMuiTheme()}>
      <div>
        <Header {...props.header}></Header>
        <MenuTiles {...props.menuTiles} />
        <section className="section section-small-padding catalog">
          <h1 className="subtitle is-4">{props.title}</h1>
          <div>
            {props.newUrl &&
              <a href={props.newUrl} className="button is-primary">
                <span className="icon">
                  <Plus />
                </span>
                <span>
                  {props.newText}
                </span>
              </a>
            }
          </div>
          <div>
            <div className="catalog__search is-flex-centered">
              <TextField id="search" className="catalog__search-field" label={props.searchLabel} type="search" autoComplete="off" />
              <Button type="button" variant="contained" color="primary">
                {props.searchLabel}
              </Button>
            </div>
            {(props.pagedProducts && props.pagedProducts.data) ?
              <div className="table-container">
                <div className="catalog__table">
                  <TableContainer component={Paper}>
                    <Table aria-label={props.title}>
                      <TableHead>
                        <TableRow>
                          <TableCell width="11%"></TableCell>
                          <TableCell>{props.skuLabel}</TableCell>
                          <TableCell>{props.nameLabel}</TableCell>
                          <TableCell>{props.lastModifiedDateLabel}</TableCell>
                          <TableCell>{props.createdDateLabel}</TableCell>
                        </TableRow>
                      </TableHead>
                      <TableBody>
                        {props.pagedProducts.data.map((product) => (
                          <TableRow key={product.name}>
                            <TableCell width="11%">
                              <Fab size="small" color="secondary" aria-label="Edit">
                                <EditIcon />
                              </Fab>
                              <Fab size="small" color="primary" aria-label="Delete">
                                <DeleteIcon />
                              </Fab>
                            </TableCell>
                            <TableCell>{product.sku}</TableCell>
                            <TableCell>{product.name}</TableCell>
                            <TableCell>{moment(product.lastModifiedDate).local().format('LT')}</TableCell>
                            <TableCell>{moment(product.createdDate).local().format('LT')}</TableCell>
                          </TableRow>
                        ))}
                      </TableBody>
                    </Table>
                  </TableContainer>
                </div>
                <div className="catalog__pagination is-flex-centered">
                  <TablePagination
                    labelDisplayedRows={({ count }) => `Total: ${count}`}
                    labelRowsPerPage={props.rowsPerPageLabel}
                    backIconButtonText={props.backIconButtonText}
                    nextIconButtonText={props.nextIconButtonText}
                    rowsPerPageOptions={PaginationConstants.DefaultRowsPerPage()}
                    component="div"
                    count={props.pagedProducts.total}
                    page={props.pagedProducts.pageIndex}
                    onChangePage={handleChangePage}
                    rowsPerPage={PaginationConstants.DefaultRowsPerPage()}
                    onChangeRowsPerPage={handleChangeRowsPerPage}
                  />
                </div>
              </div> :
              <div>
                {props.noResultsLabel}
              </div>
            }
          </div>
        </section>
        <Footer {...props.footer}></Footer>
      </div>
    </ThemeProvider>
  );
}

export default ProductPage;