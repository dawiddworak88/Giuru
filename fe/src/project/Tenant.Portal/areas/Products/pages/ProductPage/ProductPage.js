import React from 'react';
import { ThemeProvider } from '@material-ui/core/styles';
import Fab from '@material-ui/core/Fab';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import { Button, TextField, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, TablePagination } from '@material-ui/core';
import { Plus } from 'react-feather';
import GlobalHelper from '../../../../../../shared/helpers/globals/GlobalHelper';
import Header from '../../../../../../shared/components/Header/Header';
import Footer from '../../../../../../shared/components/Footer/Footer';
import MenuTiles from '../../../../../../shared/components/MenuTiles/MenuTiles';

/* eslint-disable no-unused-vars */
import favicon from '../../../../../../shared/layouts/images/favicon.png';
/* eslint-enable no-unused-vars */


function createData(name, calories, fat, carbs, protein) {
  return { name, calories, fat, carbs, protein };
}

const rows = [
  createData('Frozen yoghurt', 159, 6.0, 24, 4.0),
  createData('Ice cream sandwich', 237, 9.0, 37, 4.3),
  createData('Eclair', 262, 16.0, 24, 6.0),
  createData('Cupcake', 305, 3.7, 67, 4.3),
  createData('Gingerbread', 356, 16.0, 49, 3.9),
];

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
                {props.showNew &&
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
                <div className="table-container">
                    <div className="catalog__table">
                        <TableContainer component={Paper}>
                            <Table aria-label={props.title}>
                                <TableHead>
                                    <TableRow>
                                        <TableCell width="11%"></TableCell>
                                        <TableCell align="left">Dessert (100g serving)</TableCell>
                                        <TableCell align="right">Calories</TableCell>
                                        <TableCell align="right">Fat&nbsp;(g)</TableCell>
                                        <TableCell align="right">Carbs&nbsp;(g)</TableCell>
                                        <TableCell align="right">Protein&nbsp;(g)</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {rows.map((row) => (
                                        <TableRow key={row.name}>
                                            <TableCell width="11%">
                                                {props.editUrl &&
                                                <Fab size="small" color="secondary" aria-label="Edit">
                                                    <EditIcon />
                                                </Fab>
                                                }
                                                {props.deleteUrl &&
                                                <Fab size="small" color="primary" aria-label="Delete">
                                                    <DeleteIcon />
                                                </Fab>
                                                }
                                                
                                            </TableCell>
                                            <TableCell>{row.name}</TableCell>
                                            <TableCell align="right">{row.calories}</TableCell>
                                            <TableCell align="right">{row.fat}</TableCell>
                                            <TableCell align="right">{row.carbs}</TableCell>
                                            <TableCell align="right">{row.protein}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </div>
                    <div className="catalog__pagination is-flex-centered">
                        <TablePagination
                            component="div"
                            count={100}
                            page={page}
                            onChangePage={handleChangePage}
                            rowsPerPage={rowsPerPage}
                            onChangeRowsPerPage={handleChangeRowsPerPage}
                        />
                    </div>
                </div>
            </div>
        </section>
        <Footer {...props.footer}></Footer>
      </div>
    </ThemeProvider>
  );
}

export default ProductPage;