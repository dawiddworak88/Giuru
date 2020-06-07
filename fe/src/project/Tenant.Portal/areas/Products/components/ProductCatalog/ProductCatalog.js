
import React, { useContext } from 'react';
import { toast } from 'react-toastify';
import moment from 'moment';
import Fab from '@material-ui/core/Fab';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import { Button, TextField, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, TablePagination } from '@material-ui/core';
import FetchErrorHandler from '../../../../../../shared/helpers/errorHandlers/FetchErrorHandler';
import KeyConstants from '../../../../../../shared/constants/KeyConstants';
import { Context } from '../../../../../../shared/stores/Store';
import PaginationConstants from '../../../../../../shared/constants/PaginationConstants';
import QueryStringSerializer from '../../../../../../shared/helpers/serializers/QueryStringSerializer';

function ProductCatalog(props) {
  
  const [state, dispatch] = useContext(Context);
  const [page, setPage] = React.useState(0);
  const [searchTerm, setSearchTerm] = React.useState('');
  const [pagedProducts, setPagedProducts] = React.useState(props.pagedProducts);

  const handleSearchTermKeyPress = (event) => {

    if (event.key == KeyConstants.Enter()) {
      search();
    }
  }

  const handleOnChange = (event) => {
    setSearchTerm(event.target.value);
  }

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  }

  const search = () => {

    dispatch({ type: 'SET_IS_LOADING', payload: true });

    const searchParameters = {

      searchTerm: searchTerm,
      pageIndex: page + 1,
      itemsPerPage: PaginationConstants.DefaultRowsPerPage()       
    };

    const queryStringSearchParameters = QueryStringSerializer.serialize(searchParameters);

    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' }
    };

    var url = props.searchApiUrl;

    if (queryStringSearchParameters) {
      url += '?' + queryStringSearchParameters;
    }

    return fetch(url, requestOptions)
      .then(function (response) {

        dispatch({ type: 'SET_IS_LOADING', payload: false });

        FetchErrorHandler.handleUnauthorizedResponse(response);

        return response.json().then(jsonResponse => {

          if (response.ok) {
            setPage(0);
            setPagedProducts(jsonResponse.data.pagedProducts);
          }
          else {
            FetchErrorHandler.consoleLogResponseDetails(searchParameters, response, jsonResponse);
            toast.error(jsonResponse.message);
          }
        })
      }).catch(error => {
        console.log(error);
        dispatch({ type: 'SET_IS_LOADING', payload: false });
        toast.error(generalErrorMessage);
      });
  }

    return (
        <div>
            <div className="catalog__search is-flex-centered">
                <TextField id="search" name="search" value={searchTerm} onChange={handleOnChange} onKeyPress={handleSearchTermKeyPress} className="catalog__search-field" label={props.searchLabel} type="search" autoComplete="off" />
                <Button onClick={search} type="button" variant="contained" color="primary">
                    {props.searchLabel}
                </Button>
            </div>
            {(pagedProducts && pagedProducts.data && pagedProducts.data.length > 0) ?
                (<div className="table-container">
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
                                    {pagedProducts.data.map((product) => (
                                        <TableRow key={product.name}>
                                            <TableCell width="11%">
                                                <Fab size="small" color="secondary" aria-label={props.editLabel}>
                                                    <EditIcon />
                                                </Fab>
                                                <Fab size="small" color="primary" aria-label={props.deleteLabel}>
                                                    <DeleteIcon />
                                                </Fab>
                                            </TableCell>
                                            <TableCell>{product.sku}</TableCell>
                                            <TableCell>{product.name}</TableCell>
                                            <TableCell>{moment(product.lastModifiedDate).local().format('L LT')}</TableCell>
                                            <TableCell>{moment(product.createdDate).local().format('L LT')}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </div>
                    <div className="catalog__pagination is-flex-centered">
                        <TablePagination
                            labelDisplayedRows={({ from, to, count }) => `${from} - ${to} ${props.displayedRowsLabel} ${count}`}
                            labelRowsPerPage={props.rowsPerPageLabel}
                            backIconButtonText={props.backIconButtonText}
                            nextIconButtonText={props.nextIconButtonText}
                            rowsPerPageOptions={PaginationConstants.DefaultRowsPerPage()}
                            component="div"
                            count={pagedProducts.total}
                            page={page}
                            onChangePage={handleChangePage}
                            rowsPerPage={PaginationConstants.DefaultRowsPerPage()}
                        />
                    </div>
                </div>) :
                (<section className="section is-flex-centered">
                    <span className="is-title is-5 is-bold">{props.noResultsLabel}</span>
                </section>)
            }
        </div>
    )
}

export default ProductCatalog;